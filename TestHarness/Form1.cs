using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StormPathUserManagement;
using System.IO;

namespace TestHarness
{
    public partial class Form1 : Form
    {
        private string apiKeyID;
        private string apiKeySecret;
        private string applicationhref = "https://api.stormpath.com/v1/applications";
        private string applicationID = "";
        private string accounthref = "https://api.stormpath.com/v1/accounts";
        private Stormpath stormPath;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] info = new string[3];

            try
            {
                info = File.ReadAllLines(@"C:\stormpathinfo\info.txt");
            }
            catch (Exception ex)
            {
                throw;
            }

            apiKeyID = info[0];
            apiKeySecret = info[1];
            applicationID = info[2];
            applicationhref += "/" + applicationID;

            txtAPIKeyID.Text = apiKeyID;
            txtAPIKeySecret.Text = apiKeySecret;

            stormPath = new Stormpath(apiKeyID, apiKeySecret, applicationhref, accounthref);

            txtEmail.Text = info[3];
            txtPassword.Text = info[4];
            txtFullName.Text = info[5];
            txtGivenName.Text = info[6];
            txtSurname.Text = info[7];
            txtStatus.Text = info[8];
            txtAccountGUID.Text = info[9];
        }


        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            try
            {
                StormpathAccount<CustomSmugglerData> stormpathSmuggler = stormPath.CreateAccount<CustomSmugglerData>(txtEmail.Text, txtPassword.Text, txtFullName.Text, txtGivenName.Text, txtSurname.Text, txtStatus.Text);

                if (stormpathSmuggler.href != null)
                {
                    txtAccountResult.Text = stormpathSmuggler.content;
                    txtAccountEmailResponse.Text = stormpathSmuggler.email;
                    txtAccountFullNameResponse.Text = stormpathSmuggler.fullName;
                    txtAccountGUIDResponse.Text = stormpathSmuggler.href.Split('/').Last();
                    txtAccountGivenNameResponse.Text = stormpathSmuggler.givenName;
                    txtAccountPasswordResponse.Text = stormpathSmuggler.password;
                    txtAccountStatusResponse.Text = stormpathSmuggler.status;
                    txtAccountSurNameResponse.Text = stormpathSmuggler.surname;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAuthenticateAccount_Click(object sender, EventArgs e)
        {
            try
            {
                var stormpathAccount = stormPath.AuthenticateAccount<dynamic>(txtEmail.Text, txtPassword.Text, GetExpandedSelections());

                if (!string.IsNullOrEmpty(stormpathAccount.href))
                {
                    txtAccountResult.Text = stormpathAccount.href;
                    MessageBox.Show("Authenticated");
                }
                else
                {
                    MessageBox.Show("Failed Authentication");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        private void btnAccountRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                var stormpathAccount = stormPath.RetrieveAccount<StormpathCustomData>(txtAccountGUID.Text, GetExpandedSelections());

                if (stormpathAccount.href != null)
                {
                    txtAccountResult.Text = stormpathAccount.content;
                    txtAccountEmailResponse.Text = stormpathAccount.email;
                    txtAccountFullNameResponse.Text = stormpathAccount.fullName;
                    txtAccountGUIDResponse.Text = stormpathAccount.href.Split('/').Last();
                    txtAccountGivenNameResponse.Text = stormpathAccount.givenName;
                    txtAccountPasswordResponse.Text = stormpathAccount.password;
                    txtAccountStatusResponse.Text = stormpathAccount.status;
                    txtAccountSurNameResponse.Text = stormpathAccount.surname;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<Stormpath.ExpansionLinkTypes> GetExpandedSelections()
        {
            var expansionLinks = new List<Stormpath.ExpansionLinkTypes>();

            if (chkExpandAccount.Checked)
            {
                expansionLinks.Add(Stormpath.ExpansionLinkTypes.account);
            }

            if (chkExpandCustomData.Checked)
            {
                expansionLinks.Add(Stormpath.ExpansionLinkTypes.customData);
            }

            if (chkExpandTenant.Checked)
            {
                expansionLinks.Add(Stormpath.ExpansionLinkTypes.tenant);
            }

            if (chkExpandDirectory.Checked)
            {
                expansionLinks.Add(Stormpath.ExpansionLinkTypes.directory);
            }

            if (chkExpandGroups.Checked)
            {
                expansionLinks.Add(Stormpath.ExpansionLinkTypes.groups);
            }

            if (chkExpandGroupMemberships.Checked)
            {
                expansionLinks.Add(Stormpath.ExpansionLinkTypes.groupmemberships);
            }

            return expansionLinks;
        }

        private void btnCreateAccountWithCustomPassengerData_Click(object sender, EventArgs e)
        {
            var newPassenger1 = new PassengerCustomData()
            {
                Id = new Guid(),
                FirstName = "Han",
                LastName = "Solo",
                Birthdate = Convert.ToDateTime("2015-03-02"),
                Address1 = "1234 Hariison Ford Road",
                Address2 = "",
                PostalCode = "12345",
                City = "Corellia",
                State = "California",
                Phone = "1234567890",
                Email = "hans@gmail.com"
            };

            var newPassenger2 = new PassengerCustomData()
            {
                Id = new Guid(),
                FirstName = "Chewbacca",
                LastName = "Wooky",
                Birthdate = Convert.ToDateTime("2015-03-02"),
                Address1 = "1234 Peter Mayhew Road",
                Address2 = "",
                PostalCode = "12345",
                City = "Kashyyyk",
                State = "California",
                Phone = "1234567890",
                Email = "cwooky@gmail.com"
            };

            var newPassenger3 = new PassengerCustomData()
            {
                Id = new Guid(),
                FirstName = "Leia",
                LastName = "Organa",
                Birthdate = Convert.ToDateTime("2015-03-02"),
                Address1 = "1234 Carrie Fisher Road",
                Address2 = "",
                PostalCode = "12345",
                City = "Alderaan",
                State = "California",
                Phone = "1234567890",
                Email = "cwooky@gmail.com"
            };

            var customDataList = new[] { newPassenger1, newPassenger2, newPassenger3 }.ToList();

            var customDataPassengerData = new CustomPassengerData()
            {
                Passengers = customDataList
            };

            //we create an anonymous type so that the serializer will only serialize what we put in.
            //If we use StorpathCustomData directly then it will have every custom type in the customData for this user.
            var customData = new
            {
                PassengerData = customDataPassengerData
            };

            try
            {
                StormpathAccount<StormpathCustomData> stormpathAccount = stormPath.CreateAccount<StormpathCustomData>(txtEmail.Text, txtPassword.Text, txtFullName.Text, txtGivenName.Text, txtSurname.Text, txtStatus.Text, customData, GetExpandedSelections());

                if (stormpathAccount.href != null)
                {
                    txtAccountResult.Text = stormpathAccount.content;
                    txtAccountEmailResponse.Text = stormpathAccount.email;
                    txtAccountFullNameResponse.Text = stormpathAccount.fullName;
                    txtAccountGUIDResponse.Text = stormpathAccount.href.Split('/').Last();
                    txtAccountGivenNameResponse.Text = stormpathAccount.givenName;
                    txtAccountPasswordResponse.Text = stormpathAccount.password;
                    txtAccountStatusResponse.Text = stormpathAccount.status;
                    txtAccountSurNameResponse.Text = stormpathAccount.surname;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdateAccount_Click(object sender, EventArgs e)
        {
            try
            {
                StormpathAccount<StormpathCustomData> stormpathAccount = stormPath.UpdateAccount<StormpathCustomData>(txtAccountGUID.Text, txtEmail.Text, txtPassword.Text, txtFullName.Text, txtGivenName.Text, txtSurname.Text, txtStatus.Text, new List<Stormpath.ExpansionLinkTypes>());

                if (stormpathAccount.href != null)
                {
                    txtAccountResult.Text = stormpathAccount.content;
                    txtAccountEmailResponse.Text = stormpathAccount.email;
                    txtAccountFullNameResponse.Text = stormpathAccount.fullName;
                    txtAccountGUIDResponse.Text = stormpathAccount.href.Split('/').Last();
                    txtAccountGivenNameResponse.Text = stormpathAccount.givenName;
                    txtAccountPasswordResponse.Text = stormpathAccount.password;
                    txtAccountStatusResponse.Text = stormpathAccount.status;
                    txtAccountSurNameResponse.Text = stormpathAccount.surname;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            if (stormPath.DeleteAccount<dynamic>(txtAccountGUID.Text))
            {
                MessageBox.Show("Account successfully deleted.");
            }
            else
            {
                MessageBox.Show("Account NOT successfully deleted.");
            }

            

        }

        private void btnCreateWithSmugglerCustomData_Click(object sender, EventArgs e)
        {
            //can use an anonymous type for serialization, but then deserialization doesn't work. So we use a custom type.
            var smugglerCredentials = new SmugglingCredentialCustomData()
            {
                LicenseNumber = "700423F4A4504E8A9955C88A8322C284"
            };

            var customDataSmugglerData = new CustomSmugglerData()
            {
                SmugglingCredential = smugglerCredentials
            };

            var customData = new
            {
                SmugglerData = customDataSmugglerData
            };

            try
            {
                StormpathAccount<StormpathCustomData> stormpathAccount = stormPath.CreateAccount<StormpathCustomData>(txtEmail.Text, txtPassword.Text, txtFullName.Text, txtGivenName.Text, txtSurname.Text, txtStatus.Text, customData, GetExpandedSelections());

                if (stormpathAccount.href != null)
                {
                    txtAccountResult.Text = stormpathAccount.content;
                    txtAccountEmailResponse.Text = stormpathAccount.email;
                    txtAccountFullNameResponse.Text = stormpathAccount.fullName;
                    txtAccountGUIDResponse.Text = stormpathAccount.href.Split('/').Last();
                    txtAccountGivenNameResponse.Text = stormpathAccount.givenName;
                    txtAccountPasswordResponse.Text = stormpathAccount.password;
                    txtAccountStatusResponse.Text = stormpathAccount.status;
                    txtAccountSurNameResponse.Text = stormpathAccount.surname;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAccountCreatePassengerAndSmugglerCustomData_Click(object sender, EventArgs e)
        {
            var newPassenger1 = new PassengerCustomData()
            {
                Id = new Guid(),
                FirstName = "Han",
                LastName = "Solo",
                Birthdate = Convert.ToDateTime("2015-03-02"),
                Address1 = "1234 Hariison Ford Road",
                Address2 = "",
                PostalCode = "12345",
                City = "Corellia",
                State = "California",
                Phone = "1234567890",
                Email = "hans@gmail.com"
            };

            var newPassenger2 = new PassengerCustomData()
            {
                Id = new Guid(),
                FirstName = "Chewbacca",
                LastName = "Wooky",
                Birthdate = Convert.ToDateTime("2015-03-02"),
                Address1 = "1234 Peter Mayhew Road",
                Address2 = "",
                PostalCode = "12345",
                City = "Kashyyyk",
                State = "California",
                Phone = "1234567890",
                Email = "cwooky@gmail.com"
            };

            var newPassenger3 = new PassengerCustomData()
            {
                Id = new Guid(),
                FirstName = "Leia",
                LastName = "Organa",
                Birthdate = Convert.ToDateTime("2015-03-02"),
                Address1 = "1234 Carrie Fisher Road",
                Address2 = "",
                PostalCode = "12345",
                City = "Alderaan",
                State = "California",
                Phone = "1234567890",
                Email = "cwooky@gmail.com"
            };

            var customDataList = new[] { newPassenger1, newPassenger2, newPassenger3 }.ToList();

            var customDataPassengerData = new CustomPassengerData()
            {
                Passengers = customDataList
            };

            var smugglerCredentials = new SmugglingCredentialCustomData()
            {
                LicenseNumber = "700423F4A4504E8A9955C88A8322C284"
            };

            var customDataSmugglerData = new CustomSmugglerData()
            {
                SmugglingCredential = smugglerCredentials
            };

            var customData = new { PassengerData = customDataPassengerData, SmugglerData = customDataSmugglerData };

            try
            {
                StormpathAccount<StormpathCustomData> stormpathAccount = stormPath.CreateAccount<StormpathCustomData>(txtEmail.Text, txtPassword.Text, txtFullName.Text, txtGivenName.Text, txtSurname.Text, txtStatus.Text, customData, GetExpandedSelections());

                if (stormpathAccount.href != null)
                {
                    txtAccountResult.Text = stormpathAccount.content;
                    txtAccountEmailResponse.Text = stormpathAccount.email;
                    txtAccountFullNameResponse.Text = stormpathAccount.fullName;
                    txtAccountGUIDResponse.Text = stormpathAccount.href.Split('/').Last();
                    txtAccountGivenNameResponse.Text = stormpathAccount.givenName;
                    txtAccountPasswordResponse.Text = stormpathAccount.password;
                    txtAccountStatusResponse.Text = stormpathAccount.status;
                    txtAccountSurNameResponse.Text = stormpathAccount.surname;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
