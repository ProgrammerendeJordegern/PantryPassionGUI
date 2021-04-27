using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.ViewModels;

namespace PantryPassion.Test.Unit.ViewModelTest
{
    public class RemoveItemViewModelTest
    {
        private RemoveItemViewModel _uut;
        private ICameraViewModel _cameraViewModel;
        private BackendConnection _backendConnection;
        private object _obj;

        [SetUp]
        public void Setup()
        {
            _cameraViewModel = Substitute.For<ICameraViewModel>();
            _backendConnection = new BackendConnection();
            _uut = new RemoveItemViewModel(_cameraViewModel, _backendConnection, 5);
            _obj = new object();
        }
    }
}
