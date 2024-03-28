namespace Webwatcher
{
    public class ConfigInterface
    {
        public void SaveConfig(string homepage, bool useDefaultHomepage)
        {
            var config = new ConfigBase(homepage, useDefaultHomepage);

            ConfigLoader.Save(config);

            //System.Windows.Forms.MessageBox.Show("abc");
        }
    }
}