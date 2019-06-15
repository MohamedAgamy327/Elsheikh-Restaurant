﻿using DAL.BindableBaseService;
using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Safe : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int _userID;
        [Required]
        public int UserID
        {
            get { return _userID; }
            set { SetProperty(ref _userID, value); }
        }

        private bool _canDelete;
        public bool CanDelete
        {
            get { return _canDelete; }
            set { SetProperty(ref _canDelete, value); }
        }

        private bool _type;
        public bool Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private string _statement;
        [Required]
        public string Statement
        {
            get { return _statement; }
            set { SetProperty(ref _statement, value); }
        }

        private decimal? _amount;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal? Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }

        private DateTime _registrationDate;
        [Required]
        public DateTime RegistrationDate
        {
            get { return _registrationDate; }
            set { SetProperty(ref _registrationDate, value); }
        }

        public virtual User User { get; set; }
    }
}
