﻿using Encountify.Models;
using Encountify.Services;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class LocationDetailViewModel : BaseViewModel
    {
        private long id;
        private string name;
        private string description;
        private double longitude;
        private double latitude;
        private string category;
        private string distance;
        private Location location;

       
        public int Id
        {
            get => (int)id;
            set
            {
                int temp = (int)id;
                SetProperty(ref temp, value);
                LoadLocationId(value);
            }
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string Distance
        {
            get => distance;
            set => SetProperty(ref distance, value);
        }

        public double Longitude
        {
            get => longitude;
            set => SetProperty(ref longitude, value);
        }

        public double Latitude
        {
            get => latitude;
            set => SetProperty(ref latitude, value);
        }

        public string Category
        {
            get => category;
            set => SetProperty(ref category, value);
        }

        public async void LoadLocationId(int Id)
        {
            try
            {
                location = await DataStore.GetAsync(Id);
                Id = location.Id;
                Name = location.Name;
                Description = location.Description;
                Longitude = location.Longitude;
                Latitude = location.Latitude;
                Distance = await DistanceCounter.GetDistance(location);
                Category = CategoryConverter.ConvertCategoryToString((Category)location.Category);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Location");
            }
        }
    }
}
