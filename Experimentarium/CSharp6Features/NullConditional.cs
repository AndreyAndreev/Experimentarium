namespace Experimentarium.CSharp6Features
{
    class NullConditional
    {
        public void Method()
        {
            var a = new A();

            var b = a?.a?.a?.a?.a;

            // Compiles to:
            NullConditional.A a1 = new NullConditional.A();
            NullConditional.A a2;
            if (a1 == null)
            {
                a2 = (NullConditional.A)null;
            }
            else
            {
                NullConditional.A a3 = a1.a;
                if (a3 == null)
                {
                    a2 = (NullConditional.A)null;
                }
                else
                {
                    NullConditional.A a4 = a3.a;
                    if (a4 == null)
                    {
                        a2 = (NullConditional.A)null;
                    }
                    else
                    {
                        NullConditional.A a5 = a4.a;
                        a2 = a5 != null ? a5.a : (NullConditional.A)null;
                    }
                }
            }
        }

        class A
        {
            public A a;
        }
    }
}
