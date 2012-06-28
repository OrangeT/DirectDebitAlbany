Producer/Consumer Library For Albany Direct Debit Products
==========================================================

This library allows a developer to generate output compatable with Albany Direct Debit products (initally EPay). 

License
-------

Released under a MIT license - http://www.opensource.org/licenses/mit-license.php 

Configuration
-------------

Configuration and usage are handled distinctly.  If no configuration section is used, then a default output is used consisting of the following fields:

**Record**: Destination, TransCode, Originator, Amount, Reference
**BankAccount**: SortCode, Number, Name

To use a configuration section, add the following to your app.config/web.config:

```xml
<configuration>
	<configSections>
		<section name="DirectDebit" type="OrangeTentacle.DirectDebitAlbany.DirectDebitConfiguration, DirectDebitAlbany" />
	</configSections>

        <DirectDebit>
            <BankAccount>
                <add field="SortCode" />
                <add field="Number" />
                <add field="Name" />
            </BankAccount>

            <Record>
                <add field="Destination" />
                <add field="TransCode" />
                <add field="Originator" />
                <add field="Amount" />
                <add field="Reference" />
            </Record>
        </DirectDebit>
</configuration>
```

BankAccount and Record are both optional.  See DirectDebitAlbanyTest/Sample/.config for configuration examples.

Usage
-----

### Creating Transactions

#### Record

Each output Record (big R) represents a transaction or AUDDIS instruction.  A Record consists of an originator and destination bank account, a transaction type, value and reference.  A record is created thus:

```csharp
var originator = new BankAccount(..); // See BankAccounts
var destination = new BankAccount(..);
var transcode = TransCode.Payment; // See TransCode enum
var value = 123.45m;
var reference = "Transaction Reference";

var record = new Record(originator, destination, transcode, value, reference);
```

#### Bank Account

Each BankAccount may represent a originator or destination bank account.

```csharp
var number = "12345678";
var sortcode = "123456";
var name = "Mr Marty McFly";
var bank = Bank.Other;
var bankaccount = new BankAccount(number, sortcode, name, bank);
```

The default BankAccount constructor takes a six to eight digit account number and six digit sortcode.  Optionally, a bank can be supplied (see Bank enum).  Supplying a bank allows a nine or ten digit account number to be submitted - this will be truncated to eight digits as appropiate for the supplied bank.

#### Serialization

Given a complete record, the output can then be serialized to a fixed width format.  The Line method of the SerializedRecord may be invoked in a number of ways to return the serialized line:

var serialize = record.Serialize();

var line = serialize.Line(); // Returns fields in default order.

var fields = new [] { "Originator", "TransCode" };
var line = serialize.Line(fields); // Returns provided properties.

var config = DirectDebitConfiguration.GetSection();
var line = serialize.Line(config); // Returns properies in configuration file.

Line returns a string suitable for writing to a text file.

Disclaimer
----------

Orange Tentacle accepts NO LIABILITY whatsoever, in any way, shape, or form, in this world of the next, for the stability of this library or for your financial health.  USE AT YOUR OWN RISK.

TODO
----

* Add reading of Albany Direct Debit formats.
* Release library on NuGet.
