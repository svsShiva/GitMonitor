﻿using GitMonitor.UWP.DTO;
using GitMonitor.UWP.Pages.Dialogs;
using GitMonitor.UWP.Utilities;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using System.Linq;

namespace GitMonitor.UWP.Pages
{
    public sealed partial class UntrackedReposPage : Page
    {
        public UntrackedReposPage()
        {
            this.InitializeComponent();
        }

        public List<Repo> Repos { get; set; }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            try
            {
                APIUtility APIUtility = new APIUtility();

                Repos = await APIUtility.Get<List<Repo>>(RouteUtility._getAllUnTrackedRepos);

                dgUnTrackedRepos.ItemsSource = Repos;
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void abbtnTrackRepo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //TODO check for more optimised logic
                AppBarButton item = (AppBarButton)sender;

                Repo repo = (Repo)item.DataContext;
                repo.IsUntrackedRepo = false;

                APIUtility APIUtility = new APIUtility();

                await APIUtility.Put(repo, RouteUtility._updateRepo);

                Repos.Remove(repo);
                dgUnTrackedRepos.ItemsSource = null;
                dgUnTrackedRepos.ItemsSource = Repos;

                string message = string.Format(StringUtility._repoTrackedSucessfully, repo.Name);

                await new MessageDialog(message).ShowAsync();

            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }

        private async void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tbSearch.Text.Length != 0)
                {
                    dgUnTrackedRepos.ItemsSource = Repos.Where(m => m.Name.ToLower().Contains(tbSearch.Text)).ToList();
                }
                else
                {
                    dgUnTrackedRepos.ItemsSource = Repos;
                }
            }
            catch (Exception ex)
            {
                await new ErrorDialog(ex).ShowAsync();
            }
        }
    }
}
