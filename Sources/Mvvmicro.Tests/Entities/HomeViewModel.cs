using System;
namespace Mvvmicro.Tests
{
    public class HomeViewModel : Observable
    {
        private string firstname, lastname;

        public string Firstname
        {
            get => this.firstname;
            set => this.Set(ref this.firstname, value).ThenRaise(() => this.Fullname);
        }

        public string Lastname
        {
            get => this.lastname;
            set => this.Set(ref this.lastname, value).ThenRaise(() => this.Fullname);
        }

        public string Fullname => $"{Firstname} {Lastname}";

    }
}
