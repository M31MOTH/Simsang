﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.NetworkInformation;
using System.Net;
using System.Windows.Forms;


namespace Simsang
{
  public static class UpdatesBinaries
  {

    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static bool IsUpdateAvailable()
    {
      bool lRetVal = false;
      HttpWebRequest lWebRequest = null;
      WebResponse lWebResponse = null;
      Stream lDataStream = null;
      StreamReader lReader = null;
      String lCurrentVersion = String.Empty;

      if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up))
      {
        try
        {
          lWebRequest = (HttpWebRequest)WebRequest.Create(Config.CurrentVersionURL);
          lWebResponse = lWebRequest.GetResponse();

          lDataStream = lWebResponse.GetResponseStream();
          lReader = new StreamReader(lDataStream);
          lCurrentVersion = lReader.ReadToEnd();

          if (!String.IsNullOrEmpty(lCurrentVersion) && !String.IsNullOrEmpty(Config.ToolVersion))
          {
            if (Regex.Match(Config.ToolVersion, @"^\d+\.\d+\.\d+$").Success && Regex.Match(lCurrentVersion, @"^\d+\.\d+\.\d+$").Success)
            {
              int lCurrentVersionInt = Int32.Parse(Regex.Replace(lCurrentVersion, @"[^\d]+", ""));
              int lToolVersionInt = Int32.Parse(Regex.Replace(Config.ToolVersion, @"[^\d]+", ""));

              if (lCurrentVersionInt > lToolVersionInt)
                lRetVal = true;
            } // if (Rege...
          } // if (!St...


        }
        catch (Exception lEx)
        {
          LogConsole.Main.LogConsole.pushMsg(String.Format("IsUpdateAvailable(): {0}", lEx.Message));
        }
        finally
        {
          if (lReader != null)
            lReader.Close();

          if (lWebResponse != null)
            lWebResponse.Close();
        }
      } // if (NetworkInterface...

      return (lRetVal);
    }

    #endregion

  }
}