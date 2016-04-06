using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.Smooth;

namespace TestHabanero.BO
{
    public class BOBroker
    {
        public static IEnumerable<IClassDef> GetClassDefs()
        {
            var loadClassDefs = new ClassDefCol();
            AllClassesAutoMapper.ClassDefCol = loadClassDefs;
            var allClassesAutoMapper = new AllClassesAutoMapper(new AssemblyTypeSource(typeof(Car).Assembly));
            allClassesAutoMapper.Map();
            return loadClassDefs;
        }
        public static void LoadClassDefs()
        {
            ClassDef.ClassDefs.Clear();
            var loadedClassDefs = GetClassDefs();
            ClassDef.ClassDefs.Add(loadedClassDefs);
        }
    }
}