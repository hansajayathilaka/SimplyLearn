using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplyLearn
{
	public class RegisterResponse
	{
		public int? TrainerId { get; set; }
		public RegisterErrors? Error { get; set; }

		public RegisterResponse(int trainerId)
		{
			this.TrainerId = trainerId;
		}

		public RegisterResponse(RegisterErrors? error)
		{
			this.Error = error;
		}

	}

	public enum RegisterErrors
	{
		FirstNameRequired,
		LastNameRequired,
		EmailRequired,
		NoSessionsProvided,
		NoSessionsApproved,
		TrainerDoesNotMeetStandards,
		RegistrationError
	};
}
