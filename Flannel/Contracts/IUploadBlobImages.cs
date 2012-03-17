using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Flannel.Contracts
{
	public interface IUploadBlobImages
	{
		string UploadBlob(Stream imageStream);
	}
}
