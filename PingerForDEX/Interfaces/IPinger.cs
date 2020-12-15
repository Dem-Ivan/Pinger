using PingerForDEX.Domain;
using System.Threading.Tasks;

namespace PingerForDEX.Interfaces
{
	public interface IPinger
	{	
		string ResponseMesage { get; set; }
		Task<ResponseData> CheckStatusAsync(string hostName);
		string CreateResponseMessage(string status, string hostName);
	}
}
