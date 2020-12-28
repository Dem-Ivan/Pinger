using PingerForDEX.Tools;
using System.Threading.Tasks;

namespace PingerForDEX.Interfaces
{
	public interface IPinger
	{
		Task<ResponseData> CheckStatusAsync(string hostName);		
	}
}
