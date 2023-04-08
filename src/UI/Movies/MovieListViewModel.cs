﻿using CSharpFunctionalExtensions;
using Logic.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using UI.Common;

namespace UI.Movies
{
    public class MovieListViewModel : ViewModel
    {
        private readonly MovieRepository _repository;

        public Command SearchCommand { get; }
        public Command<long> BuyAdultTicketCommand { get; }
        public Command<long> BuyChildTicketCommand { get; }
        public Command<long> BuyCDCommand { get; }
        public IReadOnlyList<Movie> Movies { get; private set; }

        public bool ForKidsOnly { get; set; }
        public double MinimumRating { get; set; }
        public bool OnCD { get; set; }

        public MovieListViewModel()
        {
            _repository = new MovieRepository();

            SearchCommand = new Command(Search);
            BuyAdultTicketCommand = new Command<long>(BuyAdultTicket);
            BuyChildTicketCommand = new Command<long>(BuyChildTicket);
            BuyCDCommand = new Command<long>(BuyCD);
        }

        private void BuyCD(long movieId)
        {
            Maybe<Movie> movieOrNothing = _repository.GetOne(movieId);
            if (movieOrNothing.HasNoValue)
            {
                return;
            }

            Movie movie = movieOrNothing.Value;
            var specification = new GenericSpecification<Movie>(Movie.HasCDVersion);
            if (specification.IsSatisfiedBy(movie))
            {
                MessageBox.Show("The movie doesn't have a CD version", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("You've bought a ticket", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BuyChildTicket(long movieId)
        {
            Maybe<Movie> movieOrNothing = _repository.GetOne(movieId);
            if (movieOrNothing.HasNoValue)
            {
                return;
            }

            Movie movie = movieOrNothing.Value;
            var isSuitableForChildren = Movie.IsSuitableForChildren.Compile();
            var specification = new GenericSpecification<Movie>(Movie.IsSuitableForChildren);
            if (!specification.IsSatisfiedBy(movie))
            {
                MessageBox.Show("The movie is not suitable for children", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("You've bought a ticket", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BuyAdultTicket(long movieId)
        {
            MessageBox.Show("You've bought a ticket", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Search()
        {
            //var expression = ForKidsOnly ? Movie.IsSuitableForChildren: x => true;
            //var expression = OnCD ? Movie.HasCDVersion : x => true;
            
            var specification = new GenericSpecification<Movie>(Movie.HasCDVersion);

            _repository.Find()
                .Where(x => x.MpaaRating <= MpaaRating.PG || ForKidsOnly)
                .Where(x => x.ReleaseDate <= DateTime.Now.AddMonths(-6) || !OnCD)
                .ToList();
            Movies = _repository.GetList(specification);
            Notify(nameof(Movies));
        }
    }
}
