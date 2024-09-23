using RDS.Core.Models.ViewModels.Company;

namespace RDS.Web.Pages.ApplicationUsers.Configurations.UserRoles
{
    // ReSharper disable once PartialTypeWithSinglePart
    public partial class ListApplicationUsersRolesPage : ComponentBase
    {
        #region Properties

        protected bool IsBusy { get; private set; }
        protected List<CompanyIdNameViewModel> Companies { get; set; } = [];
        private CompanyIdNameViewModel? SelectedCompany { get; set; }
        protected List<AllUsersViewModel> PagedApplicationUsers { get; private set; } = [];
        protected GetAllCompaniesByUserIdRequest InputModel { get; } = new();
        private bool IsAdmin { get; } = StartService.GetIsAdmin();
        private string SearchTerm { get; set; } = string.Empty;
        protected string SearchFilter { get; set; } = string.Empty;

        protected const string EditUrl = "/usuarios/editar";
        protected const string BackUrl = "/";
        protected const string Url = "/usuariosconfiguracao/lista-roles-do-usuario";
        private readonly List<string> _sourceUrl = ["/usuariosconfiguracao/usuarios-para-roles"];
        
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
            StartService.SetPageTitle("Usuários - Roles");
            await StartService.ValidateAccesByTokenAsync();
            if (!await StartService.PermissionOnlyAdminOrOwner()) return;
            LoadStartValues();
        }

        #endregion

        #region Methods

        private void LoadStartValues()
        {
            Companies = StartService.GetUserCompanies();
            if (!Companies.Any())
                return;
            
            SelectedCompany = IsAdmin
                ? Companies.OrderByDescending(x => x.CompanyId).FirstOrDefault()
                : Companies.FirstOrDefault();

            if (SelectedCompany != null)
            {
                InputModel.CompanyId = SelectedCompany.CompanyId;
                InputModel.CompanyName = SelectedCompany.CompanyName;
                StartService.SetSelectedCompanyId(SelectedCompany.CompanyId);
            }
        }

        // private async Task LoadUsersAsync(long companyIdFilter, string searchFilter)
        // {
        //     IsBusy = true;
        //     try
        //     {
        //         var result = await UserHandler.GetAllByCompanyIdAsync(
        //             new GetAllApplicationUserRequest
        //             {
        //                 CompanyId = companyIdFilter,
        //                 Filter = searchFilter,
        //                 PageSize = _pageSize
        //             });
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

        private async Task LoadUsersDiffAdminAndOwner(long companyIdFilter, string searchFilter)
        {
            IsBusy = true;
            try
            {
                var result = (IsAdmin && companyIdFilter == 9_999_999_999_999) switch
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

        protected void OnSelectedCompany()
        {
            PagedApplicationUsers = [];
            if (SelectedCompany != null) StartService.SetSelectedCompanyId(InputModel.CompanyId);
            var newCompany = StartService.GetSelectedCompanyId();
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
            await LoadUsersDiffAdminAndOwner(InputModel.CompanyId, SearchFilter);
            StateHasChanged();
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