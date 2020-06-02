﻿using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CxAnalytix.Out.MongoDBOutput
{
    internal class SCASummarySchema : MongoDBOut, ISchema
    {
        private static ILog _log = LogManager.GetLogger(typeof(SastSummarySchema));
        private IMongoCollection<BsonDocument> _coll = null;

        public bool VerifyOrCreateSchema(IMongoDatabase db, string collectionName)
        {
            _coll = MongoUtil.MakeCollection(db, collectionName);

            var opts = new CreateIndexOptions()
            {
                Background = true
            };

            opts.Name = "ProjectName-A+ScanId-D";
            if (!MongoUtil.IndexExists(_coll, opts.Name))
                _coll.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys
                    .Ascending(new StringFieldDefinition<BsonDocument, String>("ProjectName"))
                    .Descending(new StringFieldDefinition<BsonDocument, String>("ScanId"))
                    , opts));


            opts.Name = "ScanFinished-A";
            if (!MongoUtil.IndexExists(_coll, opts.Name))
                _coll.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys
                    .Ascending(new StringFieldDefinition<BsonDocument, DateTime>("ScanFinished"))
                    , opts));

            opts.Name = "RulesViolated-A";
            if (!MongoUtil.IndexExists(_coll, opts.Name))
                _coll.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys
                    .Ascending(new StringFieldDefinition<BsonDocument, Int32>("RulesViolated"))
                    , opts));

            opts.Name = "HighVulnerabilityLibraries-A";
            if (!MongoUtil.IndexExists(_coll, opts.Name))
                _coll.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys
                    .Ascending(new StringFieldDefinition<BsonDocument, Int32>("HighVulnerabilityLibraries"))
                    , opts));

            opts.Name = "MediumVulnerabilityLibraries-A";
            if (!MongoUtil.IndexExists(_coll, opts.Name))
                _coll.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys
                    .Ascending(new StringFieldDefinition<BsonDocument, Int32>("MediumVulnerabilityLibraries"))
                    , opts));

            opts.Name = "LowVulnerabilityLibraries-A";
            if (!MongoUtil.IndexExists(_coll, opts.Name))
                _coll.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys
                    .Ascending(new StringFieldDefinition<BsonDocument, Int32>("LowVulnerabilityLibraries"))
                    , opts));

            opts.Name = "LegalHigh-A";
            if (!MongoUtil.IndexExists(_coll, opts.Name))
                _coll.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys
                    .Ascending(new StringFieldDefinition<BsonDocument, Int32>("LegalHigh"))
                    , opts));

            opts.Name = "LegalMedium-A";
            if (!MongoUtil.IndexExists(_coll, opts.Name))
                _coll.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys
                    .Ascending(new StringFieldDefinition<BsonDocument, Int32>("LegalMedium"))
                    , opts));

            opts.Name = "LegalLow-A";
            if (!MongoUtil.IndexExists(_coll, opts.Name))
                _coll.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys
                    .Ascending(new StringFieldDefinition<BsonDocument, Int32>("LegalLow"))
                    , opts));

            return true;
        }

        protected override IMongoCollection<BsonDocument> GetCollection()
        {
            if (_coll == null)
                throw new InvalidOperationException("The class was not initialized correctly.");

            return _coll;
        }
    }
}
