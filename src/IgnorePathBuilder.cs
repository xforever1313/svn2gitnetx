﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svn2GitNetX
{
    public static class IgnorePathBuilder
    {
        /// <summary>
        /// Builds the regex to pass into git svn's ignore paths argument.
        /// </summary>
        /// <returns>
        /// null if no excludes or ignores are specified.
        /// Otherwise, the regex to pass into --ignore-paths.
        /// </returns>
        public static string BuildIgnorePathRegex( Options options, IEnumerable<string> branches, IEnumerable<string> tags )
        {
            if( ( options.Exclude != null ) && options.Exclude.Any() )
            {
                // Add exclude paths to the command line. Some versions of git support
                // this for fetch only, later also for init.
                List<string> regex = new List<string>();
                if( options.RootIsTrunk == false )
                {
                    if( string.IsNullOrWhiteSpace( options.SubpathToTrunk ) == false )
                    {
                        regex.Add( options.SubpathToTrunk + @"[\/]" );
                    }

                    if( ( options.NoTags == false ) && tags.Count() > 0 )
                    {
                        foreach( var t in tags )
                        {
                            regex.Add( t + @"[\/][^\/]+[\/]" );
                        }
                    }

                    if( ( options.NoBranches == false ) && branches.Count() > 0 )
                    {
                        foreach( var b in branches )
                        {
                            regex.Add( b + @"[\/][^\/]+[\/]" );
                        }
                    }
                }

                string regexStr = "^(?:" + string.Join( "|", regex ) + ")(?:" + string.Join( "|", options.Exclude ) + ")";
                return regexStr;
            }
            else
            {
                return null;
            }
        }
    }
}