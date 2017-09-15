using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace SUR.Util1
{

    public class ActiveDirectory
    {
        /// <summary>
        /// Gets GroupNames from DOMAIN\user
        /// </summary>
        /// <param name="userName">DOMAIN\user</param>
        /// <returns></returns>
        public static List<string> GetGroupNames(string userName)
        {
            var result = new List<string>();

            if (string.IsNullOrEmpty(userName))
                return result;

            var domain = userName.Split('\\').FirstOrDefault();

            var pc = new PrincipalContext(ContextType.Domain, domain);
            var identity = UserPrincipal.FindByIdentity(pc, userName);
            if (identity != null)
            {
                var src = identity.GetGroups(pc);
                src.ToList().ForEach(sr => result.Add(sr.SamAccountName));
            }
            return result;
        }

        public static bool IsMemberOf(string userName, string group)
        {
            if (string.IsNullOrEmpty(userName))
                return false;

            var domain = userName.Split('\\').FirstOrDefault();

            var pc = new PrincipalContext(ContextType.Domain, domain);
            var identity = UserPrincipal.FindByIdentity(pc, userName);
            if (identity != null)
            {
                var src = identity.GetGroups(pc);
                var found = src.FirstOrDefault(item => item.Name == @group);
                if (found != null)
                    return true;
            }
            return false;
        }

        public static bool IsMemberOf(string userName, string[] groups)
        {
            if (string.IsNullOrEmpty(userName))
                return false;

            try
            {
                var domain = userName.Split('\\').FirstOrDefault();

                var pc = new PrincipalContext(ContextType.Domain, domain);
                var identity = UserPrincipal.FindByIdentity(pc, userName);
                if (identity != null)
                {
                    if (groups.Contains(identity.EmailAddress.ToLower()))
                        return true;

                    foreach (var currentGroup in groups)
                    {
                        try
                        {
                            var group = GroupPrincipal.FindByIdentity(pc, currentGroup);
                            if (@group == null)
                                continue;

                            if (identity.IsMemberOf(@group))
                                return true;
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return false;
        }

    }
}
