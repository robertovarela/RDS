namespace RDS.Web.Pages.ApplicationUsers
{
    // ReSharper disable once PartialTypeWithSinglePart
    public partial class ListApplicationUsersPage : ComponentBase
    {
        #region Properties

        protected bool IsBusy { get; private set; }
        protected List<ApplicationUser> PagedApplicationUsers { get; private set; } = [];
        public GetAllCompaniesByUserIdRequest InputModel { get; set; } = new();
        private string SearchTerm { get; set; } = string.Empty;
        protected string SearchFilter { get; set; } = string.Empty;
        private long LoggedUserId { get; set; }
        //private long CompanyId { get; set; }
        protected List<CompanyIdNameViewModel> Companies { get; set; } = [];
        private bool IsAdmin { get; set; }
        private bool IsOwner { get; set; }
        protected const string EditUrl = "/usuarios/editar";
        protected const string SourceUrl = "/usuarios";

        private readonly int _currentPage = 1;
        private readonly int _pageSize = Configuration.DefaultPageSize;

        #endregion

        #region Services

        [Inject] public IApplicationUserHandler UserHandler { get; set; } = null!;
        [Inject] public ISnackbar Snackbar { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;

        #endregion

        #region Overrides

        protected override async Task OnInitializedAsync()
        {
            StartService.SetPageTitle("Usuários");
            await StartService.ValidateAccesByTokenAsync();
            //await StartService.SetDefaultValues();
            if (!await StartService.PermissionOnlyAdminOrOwner()) return;
            LoggedUserId = StartService.GetLoggedUserId();
            IsAdmin = await StartService.IsAdminInRolesAsync(LoggedUserId);
            IsOwner = await StartService.IsOwnerInRolesAsync(LoggedUserId);
            if (IsOwner || IsAdmin)
            {
                Companies = StartService.GetUserCompanies();
                //CompanyId = StartService.GetSelectedCompanyId();
                if (Companies.Any())
                {
                    InputModel.CompanyId = Companies.First().CompanyId;
                    InputModel.CompanyName = Companies.First().CompanyName;
                }
            }
        }

        #endregion

        #region Methods

        private async Task LoadUsers(long companyIdFilter, string searchFilter)
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

        private async Task LoadUsersDiffAdminAndOwner(long companyIdFilter, string searchFilter)
        {
            IsBusy = true;
            try
            {
                var result = IsAdmin switch
                {
                    true => await UserHandler.GetAllAsync(new GetAllApplicationUserRequest
                        { Filter = SearchFilter, PageSize = _pageSize }),
                    false => await UserHandler.GetAllByCompanyIdAsync(
                        new GetAllApplicationUserRequest
                        {
                            CompanyId = companyIdFilter,
                            Filter = searchFilter,
                            PageSize = _pageSize
                        })
                };
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

        protected void HandleKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                OnSearch();
            }
        }

        protected async void OnSearch()
        {
            await LoadUsers(InputModel.CompanyId, SearchFilter);
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
                await OnDeleteAsync(id);
                //await LoadUsers(SearchFilter);
            }

            StateHasChanged();
        }

        private async Task OnDeleteAsync(long id)
        {
            try
            {
                var request = new DeleteApplicationUserRequest { CompanyId = id };
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

                Snackbar.Add(result.Message, result.Data != null ? Severity.Success : Severity.Warning);
            }
            catch (Exception)
            {
                Snackbar.Add("Não foi possível excluir o usuário", Severity.Error);
            }
        }

        protected Func<ApplicationUser, bool> Filter => applicationUser =>
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                return true;

            if (applicationUser.Id.ToString().Equals(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            if (applicationUser.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            if (applicationUser.Email is not null &&
                applicationUser.Email.Equals(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            if (applicationUser.Cpf is not null &&
                applicationUser.Cpf.Equals(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

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