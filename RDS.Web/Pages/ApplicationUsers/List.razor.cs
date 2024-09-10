namespace RDS.Web.Pages.ApplicationUsers
{
    // ReSharper disable once PartialTypeWithSinglePart
    public partial class ListApplicationUsersPage : ComponentBase
    {
        #region Properties

        protected bool IsBusy { get; private set; }
        protected List<CompanyIdNameViewModel> Companies { get; set; } = [];
        protected List<AllUsersViewModel> PagedApplicationUsers { get; private set; } = [];
        protected GetAllCompaniesByUserIdRequest InputModel { get; } = new();
        protected bool IsAdmin { get; } = StartService.GetIsAdmin();
        private bool IsOwner { get; } = StartService.GetIsOwner();
        private string SearchTerm { get; set; } = string.Empty;
        protected string SearchFilter { get; set; } = string.Empty;

        protected const string EditUrl = "/usuarios/editar";
        protected const string BackUrl = "/";
        private readonly int _currentPage = 1;
        private readonly int _pageSize = Configuration.DefaultPageSize;

        #endregion

        #region Services

        [Inject] private IApplicationUserHandler UserHandler { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;

        #endregion

        #region Overrides

        protected override async Task OnInitializedAsync()
        {
            StartService.SetPageTitle("Usuários");
            await StartService.ValidateAccesByTokenAsync();
            if (!await StartService.PermissionOnlyAdminOrOwner()) return;
            LoadStartValues();
        }

        #endregion

        #region Methods

        private void LoadStartValues()
        {
            if (!IsOwner && !IsAdmin)
                return;

            Companies = StartService.GetUserCompanies();
            if (!Companies.Any())
                return;

            var selectedCompany = IsAdmin
                ? Companies.OrderBy(x => x.CompanyId).FirstOrDefault()
                : Companies.FirstOrDefault();

            if (selectedCompany != null)
            {
                InputModel.CompanyId = selectedCompany.CompanyId;
                InputModel.CompanyName = selectedCompany.CompanyName;
            }
        }

        private async Task LoadUsersAsync(long companyIdFilter, string searchFilter)
        {
            IsBusy = true;
            try
            {
                var result = await UserHandler.GetAllByCompanyIdAsync(
                    new GetAllApplicationUserRequest
                    {
                        CompanyId = companyIdFilter,
                        Filter = searchFilter,
                        PageSize = _pageSize
                    });
                PagedApplicationUsers = result is { IsSuccess: true, Data: not null }
                    ? result.Data
                        .Where(Filter)
                        .Skip((_currentPage - 1) * _pageSize)
                        .Take(_pageSize)
                        .ToList()
                    : [];
            }
            catch
            {
                Snackbar.Add("Não foi possível obter a lista de usuários", Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        // private async Task LoadUsersDiffAdminAndOwner(long companyIdFilter, string searchFilter)
        // {
        //     IsBusy = true;
        //     try
        //     {
        //         var result = IsAdmin switch
        //         {
        //             true => await UserHandler.GetAllAsync(new GetAllApplicationUserRequest
        //                 { Filter = SearchFilter, PageSize = _pageSize }),
        //             false => await UserHandler.GetAllByCompanyIdAsync(
        //                 new GetAllApplicationUserRequest
        //                 {
        //                     CompanyId = companyIdFilter,
        //                     Filter = searchFilter,
        //                     PageSize = _pageSize
        //                 })
        //         };
        //         PagedApplicationUsers = result is { IsSuccess: true, Data: not null }
        //             ? result.Data
        //                 .Where(Filter)
        //                 .Skip((_currentPage - 1) * _pageSize)
        //                 .Take(_pageSize)
        //                 .ToList()
        //             : [];
        //     }
        //     catch
        //     {
        //         Snackbar.Add("Não foi possível obter a lista de usuários", Severity.Error);
        //     }
        //     finally
        //     {
        //         IsBusy = false;
        //     }
        // }

        protected void OnSelectedCompany()
        {
            PagedApplicationUsers = [];
        }

        protected async Task HandleKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await OnSearch();
            }
            else if (e.Key == "Escape" || e.CtrlKey)
            {
                SearchFilter = string.Empty;
            }
        }

        protected async Task OnSearch()
        {
            await LoadUsersAsync(InputModel.CompanyId, SearchFilter);
            StateHasChanged();
        }

        protected async void OnDeleteButtonClickedAsync(long id, string name)
        {
            var result = await DialogService.ShowMessageBox(
                "ATENÇÃO",
                $"Ao prosseguir o usuário ( {id} - {name} ) será excluído. Esta é uma ação irreversível! Deseja continuar?",
                yesText: "EXCLUIR",
                cancelText: "Cancelar");

            if (result is true)
            {
                var token = await StartService.GetTokenFromLocalStorageAsync();
                await OnDeleteAsync(id, token);
                await OnSearch();
            }

            StateHasChanged();
        }

        private async Task OnDeleteAsync(long id, string token)
        {
            if(IsAdmin)
            {
                try
                {
                    var request = new DeleteApplicationUserRequest {Token = token, RoleAuthorization = IsAdmin, UserId = id };
                    var result = await UserHandler.DeleteAsync(request);
                    PagedApplicationUsers.RemoveAll(x => x.Id == id);

                    if (result is { IsSuccess: true, Data: not null })
                    {
                        PagedApplicationUsers = PagedApplicationUsers
                            .Where(Filter)
                            .Skip((_currentPage - 1) * _pageSize)
                            .Take(_pageSize)
                            .ToList();
                    }
                    else
                    {
                        PagedApplicationUsers = [];
                    }

                    Snackbar.Add(result.Message!, result.Data != null ? Severity.Success : Severity.Warning);
                }
                catch (Exception)
                {
                    Snackbar.Add("Não foi possível excluir o usuário", Severity.Error);
                }
            }
            else
            {
                Snackbar.Add("Operação não permitida", Severity.Error);
            }
        }

        protected Func<AllUsersViewModel, bool> Filter => applicationUser =>
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                return true;

            if (applicationUser.Id.ToString().Equals(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            if (applicationUser.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            if (applicationUser.Email.Equals(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            // if (applicationUser.Cpf is not null &&
            //     applicationUser.Cpf.Equals(SearchTerm, StringComparison.OrdinalIgnoreCase))
            //     return true;

            return false;
        };

        protected async void OnNewUserButton()
        {
            var result = await DialogService.ShowMessageBox(
                "ATENÇÃO",
                $"Ao prosseguir será efetuado logout para permitir o cadastro de um novo usuário. Deseja continuar?",
                yesText: "LOGOUT",
                cancelText: "Cancelar");

            if (result is true)
                OnNewUser();
        }

        private void OnNewUser()
        {
            NavigationService.NavigateToRegister();
        }

        #endregion
    }
}