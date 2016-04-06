using System;
using Habanero.BO;
using Habanero.Testability;
using TestHabanero.BO;

namespace TestHabanero.Tests.Commons
{
    public class PartBuilder
    {
        private readonly Part _part;
        public PartBuilder()
        {
            _part = BuildValid();
            _part.Name = RandomValueGen.GetRandomString();
            _part.Description = RandomValueGen.GetRandomString();
            _part.Price = RandomValueGen.GetRandomInt();
        }

        private BOTestFactory<T> CreateFactory<T>() where T : BusinessObject
        {
            BOBroker.LoadClassDefs();
            return new BOTestFactory<T>();
        }


        private Part BuildValid()
        {
            var car = CreateFactory<Part>()
                .SetValueFor(c => c.Name)
                .CreateValidBusinessObject();
            return car;
        }

        public PartBuilder WithName(string name)
        {
            _part.Name = name;
            return this;
        }
        public PartBuilder WithNewId()
        {
            _part.PartId = Guid.NewGuid();
            return this;
        }

        public Part BuildSaved()
        {
            _part.Save();
            return _part;
        }

        public Part Build()
        {

            return _part;
        }
    }
}