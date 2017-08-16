using System.Configuration;
using Xunit;
using SolrNet;
using SolrNet.Impl;
using StructureMap.Configuration.DSL;
using StructureMap.SolrNetIntegration.Config;

namespace StructureMap.SolrNetIntegration.Tests
{

    public class StructureMapMultiCoreFixture
    {
        public StructureMapMultiCoreFixture()
        {
            var solrConfig = (SolrConfigurationSection)ConfigurationManager.GetSection("solr");
            ObjectFactory.Initialize(c =>
            {
                c.Scan(s =>
                {
                    s.Assembly(typeof(SolrNetRegistry).Assembly);
                    s.Assembly(typeof(SolrConnection).Assembly);
                    s.WithDefaultConventions();
                });
                c.AddRegistry(new SolrNetRegistry(solrConfig.SolrServers));
            });
        }

        [Fact]
        public void Get_SolrOperations_for_Entity()
        {
            var solrOperations = ObjectFactory.Container.GetInstance<ISolrOperations<Entity>>();
            Assert.NotNull(solrOperations);
        }

        [Fact]
        public void Get_SolrOperations_for_Entity2()
        {
            var solrOperations2 = ObjectFactory.Container.GetInstance<ISolrOperations<Entity2>>("entity2");
            Assert.NotNull(solrOperations2);
        }

        [Fact]
        public void Same_document_type_different_core_url()
        {
            var core1 = ObjectFactory.Container.GetInstance<ISolrOperations<Entity2>>("entity2");
            var core2 = ObjectFactory.Container.GetInstance<ISolrOperations<Entity2>>("entity3");
        }
    }
}