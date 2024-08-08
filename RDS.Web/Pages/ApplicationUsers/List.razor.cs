namespace RDS.Web.Pages.ApplicationUsers
{
    public partial class ListApplicationUsersPage : ComponentBase
    {
        #region Properties

        protected bool IsBusy { get; private set; }
        protected List<ApplicationUser> ApplicationUsers { get; private set; } = new List<ApplicationUser>();
        protected List<ApplicationUser> PagedApplicationUsers { get; private set; } = new List<ApplicationUser>();
        public string SearchTerm { get; set; } = string.Empty;
        public string SearchFilter { get; set; } = string.Empty; // Campo de busca inicial
        protected const string Url = "/usuarios/editar";

        private int _currentPage = 1;
        private int _pageSize = Configuration.DefaultPageSize;

        #endregion

        #region Services

        [Inject] public IApplicationUserHandler UserHandler { get; set; } = null!;
        [Inject] public LinkUserStateService Link { get; set; } = null!;
        [Inject] public ISnackbar Snackbar { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;

        #endregion

        #region Overrides

        protected override async Task OnInitializedAsync()
        {
            await StartService.ValidateAccesByToken();
            await LoadUsers();
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
            catch (Exception)
            {
                Snackbar.Add("Não foi possível obter a lista de usuários", Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnSearch()
        {
            await LoadUsers();
            StateHasChanged();
        }
        public async void OnDeleteButtonClickedAsync(long id, string name)
        {
            var result = await DialogService.ShowMessageBox(
                "ATENÇÃO",
                $"Ao prosseguir o usuário ( {id} - {name} ) será excluído. Esta é uma ação irreversível! Deseja continuar?",
                yesText: "EXCLUIR",
                cancelText: "Cancelar");

            if (result is true)
                await OnDeleteAsync(id);

            StateHasChanged();
        }

        private async Task OnDeleteAsync(long id)
        {
            try
            {
                var request = new DeleteApplicationUserRequest { UserId = id };
                var result = await UserHandler.DeleteAsync(request);
                ApplicationUsers.RemoveAll(x => x.Id == id);
                PagedApplicationUsers = PaginateUsers(_currentPage, _pageSize);
                Snackbar.Add(result.Message, result.Data != null ? Severity.Success : Severity.Warning);
            }
            catch (Exception)
            {
                Snackbar.Add("Não foi possível excluir o usuário", Severity.Error);
            }
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