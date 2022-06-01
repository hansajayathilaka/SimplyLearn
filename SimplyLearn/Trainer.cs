using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimplyLearn
{
    public class Trainer
    {
        public int TrainerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? YearsOfExperience { get; set; }
        public bool HasBlog { get; set; }
        public string BlogURL { get; set; }
        public WebBrowser Browser { get; set; }
        public List<string> ListOfCertifications { get; set; }
        public string Employer { get; set; }
        public int RegistrationFee { get; set; }
        public List<Session> Sessions { get; set; }
        public const int ACCEPTABLE_YEARS_OF_EXPERINCE = 10;
        public const int ACCEPTABLE_NUMBER_OF_CERTIFICATIONS = 3;
        public const int ACCEPTABLE_BROWSER_VERSION = 9;

        /// Register a Trainer
        public RegisterResponse RegisterTrainer(IRepository repository)
        {
            ValidateRegistration();
            if (IsSessionApproved() || SpecialCircumstancesToApprove())
            {
                RegistrationFee = CalculateRegistrationFee();
                SaveTrainer(repository);
                return new RegisterResponse((int)TrainerId);
            }
            return new RegisterResponse(RegisterErrors.NoSessionsApproved);
        }


        public RegisterResponse ValidateRegistration()
        {
            ValidateTrainerData();
            if (!TrainerMeetsRequirements())
            {
                SpecialCircumstancesToApprove();
                return new RegisterResponse(RegisterErrors.TrainerDoesNotMeetStandards);
            }
            return new RegisterResponse(RegisterErrors.RegistrationError);
        }

        public RegisterResponse ValidateTrainerData()
        {
            if (string.IsNullOrWhiteSpace(FirstName)) return new RegisterResponse(RegisterErrors.FirstNameRequired);
            if (string.IsNullOrWhiteSpace(LastName)) return new RegisterResponse(RegisterErrors.LastNameRequired);
            if (string.IsNullOrWhiteSpace(Email)) return new RegisterResponse(RegisterErrors.EmailRequired);
            if (Sessions.Count() == 0) return new RegisterResponse(RegisterErrors.NoSessionsProvided);
            return new RegisterResponse(RegisterErrors.RegistrationError);
        }

        public bool TrainerMeetsRequirements()
        {
            if (YearsOfExperience > ACCEPTABLE_YEARS_OF_EXPERINCE) return true;
            if (HasBlog) return true;
            if (ListOfCertifications.Count() > ACCEPTABLE_NUMBER_OF_CERTIFICATIONS) return true;
            if (HasPreviousEmployment()) return true;
            return false;
        }

        public bool HasPreviousEmployment()
        {
            var previousEmployers = new List<string>() { "Salesforce", "Microsoft", "Google", "Amazon" };
            return previousEmployers.Contains(Employer);
        }

        public bool SpecialCircumstancesToApprove()
        {
            if (!HasBlacklistedEmailDomain() && (!HasOutdatedBrowserAndMajorVersion())) return true;
            return false;
        }

        public bool HasBlacklistedEmailDomain()
        {
            var listOfDomains = new List<string>() { "gmail.com", "yahoo.com", "hotmail.com" };
            string emailDomain = Email.Split('@').Last();
            if (listOfDomains.Contains(emailDomain)) return true;
            return false;
        }

        public bool HasOutdatedBrowserAndMajorVersion()
        {
            if (Browser.BrowserName == WebBrowser.BrowserNames.InternetExplorer && Browser.MajorVersion < ACCEPTABLE_BROWSER_VERSION) return true;
            return false;

        }

        public bool IsSessionApproved()
        {
            bool isSessionApproved = true;
            var listOfOutdatedProgrammingLanguages = new List<string> { "vb6", "assembly", "forrtan", "VBScript" };
            foreach (var session in Sessions)
            {
                foreach (var technology in listOfOutdatedProgrammingLanguages)
                {
                    if (session.Title.Contains(technology) || session.Description.Contains(technology))
                    {
                        session.Approved = false;
                        isSessionApproved = false;
                    }
                    else
                    {
                        //if we got this far, the speaker is approved                      
                        session.Approved = true;
                        isSessionApproved = true;
                    }
                }
            }
            return isSessionApproved;
        }

        public int CalculateRegistrationFee()
        {
            //First, let's calculate the registration fee. 
            //More experienced speakers pay a lower fee.								
            if (YearsOfExperience <= 1)
            {
                return RegistrationFee = 500;
            }
            else if (YearsOfExperience >= 2 && YearsOfExperience <= 3)
            {
                return RegistrationFee = 250;
            }
            else if (YearsOfExperience >= 4 && YearsOfExperience <= 5)
            {
                return RegistrationFee = 100;
            }
            else if (YearsOfExperience >= 6 && YearsOfExperience <= 9)
            {
                return RegistrationFee = 50;
            }
            else
            {
                return RegistrationFee = 0;
            }
        }

        public void SaveTrainer(IRepository repository)
        {
            //Now, save the speaker and sessions to the db.
            try
            {
                Thread a = new Thread(() => repository.SaveTrainer(this));
                a.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
