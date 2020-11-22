
using System;
using System.Threading.Tasks;

namespace PingerForDEX.Interfaces
{
	public interface IPinger
	{
		event Action<string> ChangeStatus;
		string ResponseMesage { get; set; }
		Task<string> CheckStatusAsynk();
		string CreateResponseMessage(string status);
	}
}
