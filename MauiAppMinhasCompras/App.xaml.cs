﻿namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.ListarProduto());
        }

        /*protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }*/
    }
}