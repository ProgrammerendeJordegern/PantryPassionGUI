using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.ViewModels;
using PantryPassionGUI.Views;

namespace PantryPassion.Test.Unit.ViewModelTest
{
    [TestFixture]
    class MainWindowViewModelTest
    {
        private MainWindowViewModel _uut;
        private object _obj;

        [SetUp]
        public void Setup()
        {
            _uut = new MainWindowViewModel();
            _obj = new object();
        }

        //[Test]
        //public void MainWindowViewModel_AddItemCommand_CanExecute()
        //{
        //    Assert.That(_uut.AddItemCommand.CanExecute(_obj), Is.True);
        //}

        //[Test]
        //public void MainWindowViewModel_RemoveItemCommand_CanExecute()
        //{
        //    Assert.That(_uut.RemoveItemCommand.CanExecute(_obj), Is.True);
        //}

        //[Test]
        //public void MainWindowViewModel_ShoppingListCommand_CanExecute()
        //{
        //    Assert.That(_uut.ShoppingListCommand.CanExecute(_obj), Is.True);
        //}

        //[Test]
        //public void MainWindowViewModel_FindItemCommand_CanExecute()
        //{
        //    Assert.That(_uut.FindItemCommand.CanExecute(_obj), Is.True);
        //}


    }
}