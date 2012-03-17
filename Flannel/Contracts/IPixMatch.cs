using System.IO;
using Flannel.Transport;

namespace Flannel.Contracts
{
	public interface IPixMatch
	{
		void Add(Stream images);
		SearchResult[] Search(Stream image);
		bool IsDuplicate(Stream image);
	}
}
