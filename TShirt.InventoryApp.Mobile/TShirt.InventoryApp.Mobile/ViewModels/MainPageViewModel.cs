using System;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TShirt.InventoryApp.Mobile.Infrastructure;
using TShirt.InventoryApp.Mobile.Models;
using TShirt.InventoryApp.Mobile.Models.Menu;
using TShirt.InventoryApp.Mobile.Pages;
using TShirt.InventoryApp.Mobile.Pages.Count;
using TShirt.InventoryApp.Mobile.Pages.Recepcion;
using Xamarin.Forms;


namespace TShirt.InventoryApp.Mobile.ViewModels
{

    public class SelectCategoryViewModel
    {
        public Category Category { get; set; }
        public bool Selected { get; set; }
    }


    public class MainPageViewModel : BindableObject
    {
        public ObservableCollection<Grouping<SelectCategoryViewModel, Item>> Categories { get; set; }

        public DelegateCommand<Grouping<SelectCategoryViewModel, Item>> HeaderSelectedCommand
        {
            get
            {
                return new DelegateCommand<Grouping<SelectCategoryViewModel, Item>>(g =>
                {

                    if (g == null) return;
                    g.Key.Selected = !g.Key.Selected;
                    if (g.Key.Selected)
                    {
                        var x =
                            DataMenu.DataItems.Where(i => (i.Category.Id == g.Key.Category.Id)).ToList();
                        foreach (var rt in x)
                        {
                            g.Add(rt);
                        }
                        //DataFactory.DataItems.Where(i => (i.Category.CategoryId == g.Key.Category.CategoryId)).ToL;
                        //DataFactory.DataItems.Where(i => (i.Category.CategoryId == g.Key.Category.CategoryId))
                        //    .ForEach(g.Add);
                    }
                    else
                    {
                        g.Clear();
                    }
                });
            }
        }

        public MainPageViewModel()
        {
            Categories = new ObservableCollection<Grouping<SelectCategoryViewModel, Item>>();
            var selectCategories =
                DataMenu.DataItems.Select(x => new SelectCategoryViewModel {Category = x.Category, Selected = false})
                    .GroupBy(sc => new {sc.Category.Id})
                    .Select(g => g.First())
                    .ToList();
            foreach (var row in selectCategories)
            {
                Categories.Add(new Grouping<SelectCategoryViewModel, Item>(row, new List<Item>()));
            }
            //selectCategories.ForEach(sc => Categories.Add(new Grouping<SelectCategoryViewModel, Item>(sc, new List<Item>())));
        }

        public int Id { get; set; }


        public ICommand NavigateCommand
        {
            get { return new RelayCommand(Navigate); }
        }

        private void Navigate()
        {
            switch (Id)
            {
                case 1:
                    Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Search());
                    break;
                case 2:
                    Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new PlantList());
                    break;
                case 3:
                    Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new PlantList());
                    break;
                case 4:
                    Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(
                        new Pages.Transaction.Sample.SubMenu());
                    break;
                default:
                    break;
            }


        }
    }

}