using System.Linq;
using LiteDB;
using Xamarin.Forms;

namespace DemoLiteDB
{
	public partial class MainPage : ContentPage
	{

        LiteDatabase _dataBase;
	    LiteCollection<Customer> Customers;
        public MainPage()
		{
			InitializeComponent();

		    _dataBase = new LiteDatabase(DependencyService.Get<IHelper>().GetFilePath("Banco.db"));
		    Customers = _dataBase.GetCollection<Customer>();
		    ListCustomers.ItemsSource = Customers.FindAll();
            BindingContext = this;
		}

        private void Insert(object sender, System.EventArgs e)
        {
            int idCustomer = Customers.Count() == 0 ? 1 : (int) (Customers.Max(x => x.Id) + 1);
            
            Customer customer = new Customer
            {
                Id = idCustomer,
                Name = EntryName.Text,
            };

            Customers.Insert(customer);
            
            ListCustomers.ItemsSource = Customers.FindAll();

        }

        private void Get(object sender, System.EventArgs e)
        {
            Customers = _dataBase.GetCollection<Customer>();

            if (Customers.Count() > 0)
            {
                var customer = Customers.FindAll().FirstOrDefault(x => x.Name == EntryName.Text);
                DisplayAlert("id: " +customer?.Id, "Name: "+customer?.Name, "ok");
            }
        }

        private async void List_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            var action = await DisplayActionSheet("Atenção", "Não", "Sim", "Deletar ?");

            if (action == "Sim")
            {
                var customer = e.SelectedItem as Customer;

                Customers.Delete(customer?.Id);

                ListCustomers.ItemsSource = Customers.FindAll();
            }
        }
    }
}
