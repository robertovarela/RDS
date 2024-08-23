namespace RDS.Web.Pages.ApplicationUsers.Configurations.UserRoles
{
    // ReSharper disable once PartialTypeWithSinglePart
    public partial class ListApplicationUsersRolesPage : ComponentBase
    {
        #region Properties

        protected bool IsBusy { get; private set; }
        private List<ApplicationUser> ApplicationUsers { get; set; } = [];
        protected List<ApplicationUser> PagedApplicationUsers { get; private set; } = [];
        protected string SearchTerm { get; set; } = string.Empty;
        protected string SearchFilter { get; set; } = string.Empty;
        protected const string Url = "/usuariosconfiguracao/roles-do-usuario/lista-roles-do-usuario";
        private readonly List<string> _sourceUrl = ["/usuariosconfiguracao/usuarios-roles"];

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
            StartService.SetPageTitle("Usuários - Roles");
            await StartService.ValidateAccesByToken();
            StartService.SetSourceUrl(_sourceUrl);
        }

        #endregion

        #region Methods

        private async Task LoadUsers()
        {
            IsBusy = true;
            try
            {
                var request = new GetAllApplicationUserRequest { Filter = SearchFilter, PageSize = _pageSize };
                var result = await UserHandler.GetAllAsync(request);
                if (result.IsSuccess)
                {
                    ApplicationUsers = result.Data ?? [];
                    PagedApplicationUsers = PaginateUsers(_currentPage, _pageSize);
                }
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

        public async void OnNewUserButton()
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

        private List<ApplicationUser> PaginateUsers(int currentPage, int pageSize)
        {
            return ApplicationUsers
                .Where(Filter)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        #endregion
    }
}