using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flannel.Contracts
{
	public interface IModeDecider
	{
		Settings.AppMode GetMode();
	}
}
