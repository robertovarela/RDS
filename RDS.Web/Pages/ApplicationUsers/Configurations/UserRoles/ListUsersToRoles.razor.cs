namespace RDS.Web.Pages.ApplicationUsers.Configurations.UserRoles
{
    // ReSharper disable once PartialTypeWithSinglePart
    public partial class ListApplicationUsersRolesPage : ComponentBase
    {
        #region Properties

        protected bool IsBusy { get; private set; }
        protected List<ApplicationUser> PagedApplicationUsers { get; private set; } = [];
        protected string SearchTerm { get; set; } = string.Empty;
        protected string SearchFilter { get; set; } = string.Empty;
        private long LoggedUserId { get; set; }
        private long CompanyId { get; set; }
        private bool IsAdmin { get; set; }
        private bool IsOwner { get; set; }
        protected const string Url = "/usuariosconfiguracao/roles-do-usuario/lista-roles-do-usuario";
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
            if(!await StartService.PermissionOnlyAdminOrOwner()) return;
            LoggedUserId = StartService.GetLoggedUserId();
            CompanyId = StartService.GetSelectedCompanyId();
            IsAdmin = await StartService.IsAdminInRolesAsync(LoggedUserId);
            IsOwner = await StartService.IsOwnerInRolesAsync(LoggedUserId);
            StartService.SetSourceUrl(_sourceUrl);
        }

        #endregion

        #region Methods

        private async Task LoadUsers()
        {
            IsBusy = true;
            try
            {
                var result = IsAdmin switch
                {
                    true => await UserHandler.GetAllAsync(new GetAllApplicationUserRequest{ Filter = SearchFilter, PageSize = _pageSize }),
                    false => await UserHandler.GetAllByCompanyIdAsync(
                        new GetAllApplicationUserRequest
                        {
                            CompanyId = CompanyId,
                            Filter = SearchFilter,
                            PageSize = _pageSize 
                        })
                };
                
                PagedApplicationUsers = result.IsSuccess ? PaginateUsers(result.Data, _currentPage, _pageSize) : [];
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

        public async void OnSearch()
        {
            await LoadUsers();
            StateHasChanged();
        }

        public Func<ApplicationUser, bool> Filter => applicationUser =>
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                return true;

            if (applicationUser.Id.ToString().Equals(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            if (applicationUser.Name is not null &&
                applicationUser.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            if (applicationUser.Email is not null &&
                applicationUser.Email.Equals(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            if (applicationUser.Cpf is not null &&
                applicationUser.Cpf.Equals(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        private List<ApplicationUser> PaginateUsers(List<ApplicationUser> users,int currentPage, int pageSize)
        {
            return users
                .Where(Filter)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        #endregion
    }
}