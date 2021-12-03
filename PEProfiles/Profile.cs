using System.IO;
using YamlDotNet.Serialization;
using Models;

namespace PEProfiles
{
    public class Profile
    {
        public static PEProfile pe = new PEProfile();
        public static bool Execute(string filename)
        {
            if (File.Exists(filename))
            {
                var yamlDotNet = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();
                pe = yamlDotNet.Deserialize<PEProfile>(File.ReadAllText(filename));
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
