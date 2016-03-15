namespace Experimentarium.CSharp6Features
{
    public class AutoProperties
    {
        public string Property { get; set; } = "hello";
        /*
        Compiles to:
        in .ctor: this.<Property>k__BackingField = "hello";

        */


        private string field;
        public string ReadOnlyProperty => field;
        /*
        Compiles to:

        public string ReadOnlyProperty
		{
			get
			{
				return this.field;
			}
		}
        
        */

        public AutoProperties()
        {
            //
        }
    }
}
