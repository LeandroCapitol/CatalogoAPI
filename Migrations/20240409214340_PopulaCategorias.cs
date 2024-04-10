﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert Into Categorias(Nome, ImagemUrl) Values ('Bebidas','bedidas.jpg')");
            mb.Sql("Insert Into Categorias(Nome, ImagemUrl) Values ('Lanches','lanches.jpg')");
            mb.Sql("Insert Into Categorias(Nome, ImagemUrl) Values ('Sobremesas','sobremesas.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
        }
    }
}
