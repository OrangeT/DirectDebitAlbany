using System;
using System.IO;
using System.Configuration;
using OrangeTentacle.DirectDebitAlbany;
using Xunit;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    public class DirectDebitConfigurationTest
    {
        public const string COMPLETE = "../../Sample/complete.config";
        public const string NO_BANKACCOUNT = "../../Sample/nobankaccount.config";
        public const string NO_RECORD = "../../Sample/norecord.config";
        public const string NO_CONFIG = "../../Sample/noconfiguration.config";
        public const string REAL = "../../Sample/real.config";

        public class FilesExist
        {
            [Fact]
            public void Complete()
            {
                var exist = File.Exists(COMPLETE);

                Assert.True(exist);
            }

            [Fact]
            public void NoBankAccount()
            {
                var exist = File.Exists(NO_BANKACCOUNT);

                Assert.True(exist);
            }

            [Fact]
            public void NoRecord()
            {
                var exist = File.Exists(NO_RECORD);

                Assert.True(exist);
            }

            [Fact]
            public void NoConfiguration()
            {
                var exist = File.Exists(NO_CONFIG);

                Assert.True(exist);
            }
        }

        public class FromConfigFile
        {
            [Fact]
            public void Complete()
            {
                var section = GetConfiguration(COMPLETE);

                Assert.True(section.BankAccount.Count > 0);
                Assert.True(section.Record.Count > 0);
            }

            [Fact]
            public void NoBankAccount()
            {
                var section = GetConfiguration(NO_BANKACCOUNT);

                Assert.Equal(0, section.BankAccount.Count);
                Assert.True(section.Record.Count > 0);
            }

            [Fact]
            public void NoRecord()
            {
                var section = GetConfiguration(NO_RECORD); 

                Assert.True(section.BankAccount.Count > 0);
                Assert.Equal(0, section.Record.Count);
            }

            [Fact]
            public void NoConfiguration()
            {
                var section = GetConfiguration(NO_CONFIG); 

                Assert.Equal(0, section.BankAccount.Count);
                Assert.Equal(0, section.Record.Count);
            }

            public static DirectDebitConfiguration GetConfiguration(string filename)
            {
                var fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = filename;
                var manager = ConfigurationManager.OpenMappedExeConfiguration(fileMap, 
                        ConfigurationUserLevel.None);

                return manager.GetSection(DirectDebitConfiguration.SECTION_NAME) 
                    as DirectDebitConfiguration;
            }
        } 

        public class Record
        {
            [Fact]
            public void Fields_In_Order()
            {
                var section = FromConfigFile.GetConfiguration(REAL);

                Assert.Equal("TransCode", section.Record[0].Field);
                Assert.Equal("Destination.SortCode", section.Record[1].Field);
                Assert.Equal("Destination.Number", section.Record[2].Field);
                Assert.Equal("Amount", section.Record[3].Field);
                Assert.Equal("Blank", section.Record[4].Field);
                Assert.Equal("Blank", section.Record[5].Field);
                Assert.Equal("Blank", section.Record[6].Field);
                Assert.Equal("Blank", section.Record[7].Field);
                Assert.Equal("Blank", section.Record[8].Field);
                Assert.Equal("Destination.Name", section.Record[9].Field);
                Assert.Equal("Reference", section.Record[10].Field);
            }
        }
    }
}
