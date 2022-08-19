using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    public partial class InsertAuthorsAndBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO [dbo].[Authors]
            VALUES
            ('Henry Fielding',
                47,
                'London',
                7
            ),
            ('Jane Austen',
                    52,
                    'Steventon, Hampshire',
                    6
                ),
            ('Charles John Huffam Dickens',
                    58,
                    'Landport ,Portsea Island',
                    22
                );
            INSERT INTO[dbo].[Books]
            VALUES
            ('The Pickwick Papers',
                10,
                3,
                (
                    SELECT MAX(ID)
            
            FROM[dbo].[Authors]
            
            WHERE Name = 'Charles John Huffam Dickens'
                ), 
            1837
                ),
            ('Oliver Twist',
                8,
                4,
                (
                    SELECT MAX(ID)
            FROM[dbo].[Authors]
            WHERE Name = 'Charles John Huffam Dickens'
                ), 
            1839
                ),
            ('Nicholas Nickleb',
                14,
                4,
                (
                    SELECT MAX(ID)
            FROM[dbo].[Authors]
            WHERE Name = 'Charles John Huffam Dickens'
                ), 
            1839
                ),
            ('The Old Curiosity Shop',
                14,
                4,
                (
                    SELECT MAX(ID)
            FROM[dbo].[Authors]
            WHERE Name = 'Charles John Huffam Dickens'
                ), 
            1841
                ),
            ('The Chimes',
                14,
                5,
                (
                    SELECT MAX(ID)
            FROM[dbo].[Authors]
            WHERE Name = 'Charles John Huffam Dickens'
                ), 
            1844
                ),
            ('Sense and Sensibility',
                13,
                4,
                (
                    SELECT MAX(ID)
            FROM[dbo].[Authors]
            WHERE Name = 'Jane Austen'
                ), 
            1811
                ),
            ('Pride and Prejudice',
                13,
                8,
                (
                    SELECT MAX(ID)
            FROM[dbo].[Authors]
            WHERE Name = 'Jane Austen'
                ), 
            1813
                ),
            ('Northanger Abbey',
                13,
                16,
                (
                    SELECT MAX(ID)
            FROM[dbo].[Authors]
            WHERE Name = 'Jane Austen'
                ), 
            1818
                ),
            ('Shamela',
                13,
                2,
                (
                    SELECT MAX(ID)
            FROM[dbo].[Authors]
            WHERE Name = 'Henry Fielding'
                ), 
            1741
                ),
            ('The Life and Death of Jonathan Wild, the Great',
                6,
                8,
                (
                    SELECT MAX(ID)
            FROM[dbo].[Authors]
            WHERE Name = 'Henry Fielding'
                ), 
            1743
                ),
            ('The History of Tom Jones, a Foundling',
                16,
                8,
                (
                    SELECT MAX(ID)
            FROM[dbo].[Authors]
            WHERE Name = 'Henry Fielding'
                ), 
            1749
                );");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
