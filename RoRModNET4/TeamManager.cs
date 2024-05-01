using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoRModNET4
{
    internal class TeamManager : MonoBehaviour
    {
        public TeamManager()
        {

        }
        private List<NetworkUser> GetTeam()
        {
            List<NetworkUser> result = new List<NetworkUser>();
            foreach (NetworkUser netuser in NetworkUser.FindObjectsOfType(typeof(NetworkUser)))
            {
                /*if (netuser.masterController.master == LocalPlayer)
                {
                    continue;
                }*/
                result.Add(netuser);
            }
            return result;
        }
    }
}
