module ShopAdmin {
    export interface IAppScope extends ng.IScope {
        toggleSidenav(menuId: string): void;
    }
}