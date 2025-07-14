using SistemaZero.Model.Entity;

using MySql.Data.MySqlClient;

namespace SistemaZero.Model
{
    class ConexaoBD
    {
        private readonly string string_conexao = "Server=localhost;Database=bd_zero;User=root;Password=;";

        private MySqlConnection Conectar()
        {
            return new MySqlConnection(string_conexao);
        }

        #region Users
        public User? ObterLogin(string email, string senha)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string logon_query = "SELECT * FROM usuario WHERE email = @EMAIL AND senha = @SENHA";
                    MySqlParameter[] parametros = { new MySqlParameter("@EMAIL", email), new MySqlParameter("@SENHA", senha) };
                    using (MySqlCommand cmd = new MySqlCommand(logon_query, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    ID = reader.GetInt32("id"),
                                    Nome = reader.GetString("nome"),
                                    Email = reader.GetString("email"),
                                    Cargo = (Cargo)reader.GetInt32("cargo"),
                                    Ativo = reader.GetBoolean("ativo"),
                                    Registro = DateOnly.FromDateTime(reader.GetDateTime("registro"))
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar usuário no banco de dados.", ex);
            }
        }

        public User? ObterUsuarioPorIdSenha(int id, string senha)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = "SELECT * FROM usuario WHERE id = @ID AND senha = @SENHA";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.Parameters.AddWithValue("@SENHA", senha);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    ID = reader.GetInt32("id"),
                                    Nome = reader.GetString("nome"),
                                    Email = reader.GetString("email"),
                                    Cargo = (Cargo)reader.GetInt32("cargo"),
                                    Ativo = reader.GetBoolean("ativo"),
                                    Registro = DateOnly.FromDateTime(reader.GetDateTime("registro"))
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao verificar senha do usuário.", ex);
            }
        }

        public User? ObterUsuario(int id)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string logon_query = "SELECT * FROM usuario WHERE id = @ID";
                    MySqlParameter[] parametros = { new MySqlParameter("@ID", id) };
                    using (MySqlCommand cmd = new MySqlCommand(logon_query, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    ID = reader.GetInt32("id"),
                                    Nome = reader.GetString("nome"),
                                    Email = reader.GetString("email"),
                                    Cargo = (Cargo)reader.GetInt32("cargo"),
                                    Ativo = reader.GetBoolean("ativo"),
                                    Registro = DateOnly.FromDateTime(reader.GetDateTime("registro"))
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar usuário no banco de dados.", ex);
            }
        }

        public List<User> PesquisarUsuario(int id_atual, int qtd_retorno, string pesq)
        {
            try
            {
                List<User> usuarios = new List<User>();
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();

                    string query = @"
                SELECT * FROM usuario
                WHERE id > @ID_ATUAL
                  AND (nome LIKE @PESQ OR email LIKE @PESQ)
                ORDER BY id";

                    if (qtd_retorno > 0)
                        query += $" ASC LIMIT {qtd_retorno}";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID_ATUAL", id_atual);
                        cmd.Parameters.AddWithValue("@PESQ", $"%{pesq}%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                usuarios.Add(new User
                                {
                                    ID = reader.GetInt32("id"),
                                    Nome = reader.GetString("nome"),
                                    Email = reader.GetString("email"),
                                    Cargo = (Cargo)reader.GetInt32("cargo"),
                                    Ativo = reader.GetBoolean("ativo"),
                                    Registro = DateOnly.FromDateTime(reader.GetDateTime("registro"))
                                });
                            }
                        }
                    }
                }

                return usuarios;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar usuários no banco de dados.", ex);
            }
        }


        public List<User> ObterUsuarios(int id_atual, int qtd_retorno)
        {
            try
            {
                List<User> usuarios = new List<User>();
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = "SELECT * FROM usuario WHERE id > @ID_ATUAL ORDER BY id";
                    if (qtd_retorno > 0)
                        query += $" ASC LIMIT {qtd_retorno}";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID_ATUAL", id_atual);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                usuarios.Add(new User
                                {
                                    ID = reader.GetInt32("id"),
                                    Nome = reader.GetString("nome"),
                                    Email = reader.GetString("email"),
                                    Cargo = (Cargo)reader.GetInt32("cargo"),
                                    Ativo = reader.GetBoolean("ativo"),
                                    Registro = DateOnly.FromDateTime(reader.GetDateTime("registro"))
                                });
                            }
                        }
                    }
                }

                return usuarios;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar usuários no banco de dados.", ex);
            }
        }


        public bool verificaEmail(string email)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query_email = @"SELECT 1 FROM usuario WHERE email = @EMAIL LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(query_email, conn))
                    {
                        cmd.Parameters.AddWithValue("@EMAIL", email);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Email já está sendo usado
                                return false;
                            }
                        }
                    }
                }
                // Email não foi encontrado, pode usar
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar email no banco de dados.", ex);
            }
        }

        public void adicionarUsuario(string email, string nome, string senha, int cargo)
        {
            try
            {
                List<User> usuarios = new List<User>();
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = @"INSERT INTO usuario (email, nome, senha, cargo) VALUES (@EMAIL, @NOME, @SENHA, @CARGO)";
                    MySqlParameter[] parametros = {
                        new MySqlParameter("@EMAIL", email),
                        new MySqlParameter("@NOME", nome),
                        new MySqlParameter("@SENHA", senha),
                        new MySqlParameter("@CARGO", cargo)
                    };
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir o usuario no banco de dados.", ex);
            }
        }

        public void mudarStatusUsuariio(int id, bool status)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query_change = "UPDATE usuario SET ativo = @STATUS WHERE id = @ID;";
                    MySqlParameter[] parametros = { new MySqlParameter("@ID", id), new MySqlParameter("@STATUS", !status) };
                    using (MySqlCommand cmd = new MySqlCommand(query_change, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao alterar status do usuário no banco de dados.", ex);
            }
        }

        public void TrocarSenhaUsuario(int id, string novaSenha)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = "UPDATE usuario SET senha = @SENHA WHERE id = @ID";
                    MySqlParameter[] parametros = {
                new MySqlParameter("@SENHA", novaSenha),
                new MySqlParameter("@ID", id)
            };

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao trocar a senha do usuário.", ex);
            }
        }

        public void editarUsuario(int id, string email, string nome, int cargo)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query_edit = "UPDATE usuario SET email = @EMAIL, nome = @NOME, cargo = @CARGO WHERE id = @ID";
                    MySqlParameter[] parametros = {
                        new MySqlParameter("@ID", id),
                        new MySqlParameter("@EMAIL", email),
                        new MySqlParameter("@NOME", nome),
                        new MySqlParameter("@CARGO", cargo)
                    };
                    using (MySqlCommand cmd = new MySqlCommand(query_edit, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar o usuário no banco de dados.", ex);
            }
        }

        public bool confirmacaoComSenha(int id, string senha)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = "SELECT 1 FROM usuario WHERE id = @ID AND senha = @SENHA LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.Parameters.AddWithValue("@SENHA", senha);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            return reader.Read(); // true se achou, false se não
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao confirmar senha no banco de dados.", ex);
            }
        }

        #endregion

        #region Produtos

        #region Adicionar Produtos
        public int adicionarProduto(Produto produto)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();

                    // Inserir produto
                    string query = @"INSERT INTO produto (descricao, categoria_id, codigo_produto) 
                             VALUES (@DESCRICAO, @CATEGORIA, @CODIGO)";
                    MySqlParameter[] parametros = {
                        new MySqlParameter("@DESCRICAO", produto.Descricao),
                        new MySqlParameter("@CATEGORIA", produto.Categoria!.Id),
                        new MySqlParameter("@CODIGO", produto.CodigoProduto)
                    };
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        cmd.ExecuteNonQuery();
                    }

                    int id;
                    using (MySqlCommand cmd = new MySqlCommand("SELECT LAST_INSERT_ID()", conn))
                    {
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (produto.Estoques != null)
                        foreach (Estoque est in produto.Estoques)
                            adicionarProdutoEstoque(est, id);

                    if (produto.Codigos_Adicionais != null)
                        foreach (Codigo_Adicional cod in produto.Codigos_Adicionais)
                            adicionarCodigo(cod, id);

                    if (produto.Imagem_Estoque != null && produto.Imagem_Estoque.Url != null)
                        adicionarImagemEstoque(produto.Imagem_Estoque, id);

                    return id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir o produto no banco de dados.", ex);
            }
        }

        public int adicionarCategoria(Categoria categoria)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();

                    // Inserir produto
                    string query = @"INSERT INTO categoria (nome, linha) 
                             VALUES (@NOME, @LINHA)";
                    MySqlParameter[] parametros = {
                        new MySqlParameter("@NOME", categoria.Nome),
                        new MySqlParameter("@LINHA", categoria.Linha)
                    };
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        cmd.ExecuteNonQuery();
                    }
                    int id;
                    using (MySqlCommand cmd = new MySqlCommand("SELECT LAST_INSERT_ID()", conn))
                    {
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    return id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir a Categoria no banco de dados.", ex);
            }
        }

        public int adicionarLocalEstoque(LocaisEstoque local)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();

                    // Inserir produto
                    string query = @"INSERT INTO estoque (nome) 
                             VALUES (@NOME)";
                    MySqlParameter[] parametros = {
                        new MySqlParameter("@NOME", local.Nome),
                    };
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        cmd.ExecuteNonQuery();
                    }
                    int id;
                    using (MySqlCommand cmd = new MySqlCommand("SELECT LAST_INSERT_ID()", conn))
                    {
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    return id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir o Estoque no banco de dados.", ex);
            }
        }

        public void adicionarProdutoEstoque(Estoque est, int id)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = @"INSERT INTO estoque_produto (produto_id, estoque_id, rua, predio, nivel, lado, quantidade) VALUES (@PRODUTO_ID, @ESTOQUE_ID, @RUA, @PREDIO, @NIVEL, @LADO, @QUANTIDADE)";
                    MySqlParameter[] parametros = {
                        new MySqlParameter("@PRODUTO_ID", id),
                        new MySqlParameter("@ESTOQUE_ID", est.Locais!.ID),
                        new MySqlParameter("@RUA", est.Rua),
                        new MySqlParameter("@PREDIO", est.Predio),
                        new MySqlParameter("@NIVEL", est.Nivel),
                        new MySqlParameter("@LADO", est.Lado),
                        new MySqlParameter("@QUANTIDADE", est.Quantidade)
                    };
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir o produto no banco de dados.", ex);
            }
        }

        public void adicionarCodigo(Codigo_Adicional cod, int id)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = @"INSERT INTO codigos_adicionais (produto_id, codigo) VALUES (@PRODUTO_ID, @CODIGO)";
                    MySqlParameter[] parametros = {
                        new MySqlParameter("@PRODUTO_ID", id),
                        new MySqlParameter("@CODIGO", cod.Codigo)
                    };
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir o produto no banco de dados.", ex);
            }
        }

        public void adicionarImagemEstoque(Imagem imagem, int produtoId)
        {
            using (var conn = Conectar())
            {
                conn.Open();

                // Inserir imagem
                string insertImagem = "INSERT INTO imagem (url) VALUES (@URL)";
                using (var cmd = new MySqlCommand(insertImagem, conn))
                {
                    cmd.Parameters.AddWithValue("@URL", imagem.Url);
                    cmd.ExecuteNonQuery();
                }

                // Obter ID da imagem
                int imagemId;
                using (var cmd = new MySqlCommand("SELECT LAST_INSERT_ID()", conn))
                {
                    imagemId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Inserir vínculo imagem_estoque
                string insertVinculo = "INSERT INTO imagem_estoque (produto_id, imagem_id) VALUES (@PRODUTO_ID, @IMAGEM_ID)";
                using (var cmd = new MySqlCommand(insertVinculo, conn))
                {
                    cmd.Parameters.AddWithValue("@PRODUTO_ID", produtoId);
                    cmd.Parameters.AddWithValue("@IMAGEM_ID", imagemId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Obter Produtos
        public Produto? obterProduto(int id)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = "SELECT * FROM produto WHERE id = @ID";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Produto
                                {
                                    ID = id,
                                    Descricao = reader.GetString("descricao"),
                                    CodigoProduto = reader.GetString("codigo_produto"),
                                    Ativo = reader.GetBoolean("ativo"),
                                    Categoria = obterCategoria(id),
                                    Imagem_Estoque = obterImagemEstoque(id),
                                    Estoques = obterEstoques(id),
                                    Codigos_Adicionais = obterCodigoAdicionals(id)
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar produto no banco de dados.", ex);
            }
        }

        public List<Produto>? obterProdutos(int id_atual, int qtd_retorno)
        {
            try
            {
                List<Produto> produtos = new List<Produto>();
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = $"SELECT * FROM produto WHERE id > @ID_ATUAL ORDER BY id";
                    if (qtd_retorno > 0) query += $" ASC LIMIT {qtd_retorno}";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID_ATUAL", id_atual);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("id");
                                produtos.Add(new Produto
                                {
                                    ID = id,
                                    Descricao = reader.GetString("descricao"),
                                    CodigoProduto = reader.GetString("codigo_produto"),
                                    Ativo = reader.GetBoolean("ativo"),
                                    Categoria = obterCategoria(id),
                                    Imagem_Estoque = obterImagemEstoque(id),
                                    Estoques = obterEstoques(id),
                                    Codigos_Adicionais = obterCodigoAdicionals(id)
                                });
                            }
                        }
                    }
                }
                return produtos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar os Produtos no banco de dados.", ex);
            }
        }

        public List<Produto>? PesquisarPorDescricao(int id_atual, int qtd_retorno, string descricao)
        {
            try
            {
                List<Produto> produtos = new List<Produto>();
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();

                    string query = @"
                        SELECT DISTINCT p.id
                        FROM produto p
                        WHERE p.id > @ID_ATUAL
                        AND p.descricao LIKE @DESCRICAO";
                    if (qtd_retorno > 0) query += $" LIMIT {qtd_retorno}";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID_ATUAL", id_atual);
                        cmd.Parameters.AddWithValue("@DESCRICAO", $"%{descricao}%");
                        cmd.Parameters.AddWithValue("@QTD_RETORNO", qtd_retorno);

                        var ids = new List<int>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                ids.Add(reader.GetInt32("id"));
                        }

                        foreach (var id in ids)
                        {
                            var produto = obterProduto(id);
                            if (produto != null)
                                produtos.Add(produto);
                        }
                    }
                }

                return produtos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar por descrição.", ex);
            }
        }

        public List<Produto>? PesquisarPorCategoria(int id_atual, int qtd_retorno, int categoriaId)
        {
            try
            {
                List<Produto> produtos = new List<Produto>();
                using (var conn = Conectar())
                {
                    conn.Open();

                    string query = @"
                        SELECT DISTINCT p.id
                        FROM produto p
                        WHERE p.id > @ID_ATUAL
                        AND p.categoria_id = @CATEGORIA_ID";
                    if (qtd_retorno > 0) query += $" LIMIT {qtd_retorno}";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID_ATUAL", id_atual);
                        cmd.Parameters.AddWithValue("@CATEGORIA_ID", categoriaId);
                        cmd.Parameters.AddWithValue("@QTD_RETORNO", qtd_retorno);

                        var ids = new List<int>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                ids.Add(reader.GetInt32("id"));
                        }

                        foreach (var id in ids)
                        {
                            var produto = obterProduto(id);
                            if (produto != null)
                                produtos.Add(produto);
                        }
                    }
                }

                return produtos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar por categoria.", ex);
            }
        }

        public List<Produto>? PesquisarPorCodigoProduto(int id_atual, int qtd_retorno, string codigo)
        {
            try
            {
                List<Produto> produtos = new List<Produto>();
                using (var conn = Conectar())
                {
                    conn.Open();

                    string query = @"
                        SELECT DISTINCT p.id
                        FROM produto p
                        WHERE p.id > @ID_ATUAL
                        AND p.codigo_produto LIKE @CODIGO";
                    if (qtd_retorno > 0) query += $" LIMIT {qtd_retorno}";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID_ATUAL", id_atual);
                        cmd.Parameters.AddWithValue("@CODIGO", $"%{codigo}%");
                        cmd.Parameters.AddWithValue("@QTD_RETORNO", qtd_retorno);

                        var ids = new List<int>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                ids.Add(reader.GetInt32("id"));
                        }

                        foreach (var id in ids)
                        {
                            var produto = obterProduto(id);
                            if (produto != null)
                                produtos.Add(produto);
                        }
                    }
                }

                return produtos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar pelo código.", ex);
            }
        }

        public List<Produto>? PesquisarPorCodigoAdicional(int id_atual, int qtd_retorno, string codigoAdicional)
        {
            try
            {
                List<Produto> produtos = new List<Produto>();
                using (var conn = Conectar())
                {
                    conn.Open();

                    string query = @"
                        SELECT DISTINCT p.id
                        FROM produto p
                        JOIN codigos_adicionais cc ON p.id = cc.produto_id
                        WHERE p.id > @ID_ATUAL
                        AND cc.codigo LIKE @CODIGO";
                    if (qtd_retorno > 0) query += $" LIMIT {qtd_retorno}";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID_ATUAL", id_atual);
                        cmd.Parameters.AddWithValue("@CODIGO", $"%{codigoAdicional}%");
                        cmd.Parameters.AddWithValue("@QTD_RETORNO", qtd_retorno);

                        var ids = new List<int>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                ids.Add(reader.GetInt32("id"));
                        }

                        foreach (var id in ids)
                        {
                            var produto = obterProduto(id);
                            if (produto != null)
                                produtos.Add(produto);
                        }
                    }
                }

                return produtos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar por código adicional.", ex);
            }
        }

        public List<Produto>? pesquisarProdutoLocal(int id_atual, int qtd_retorno, int? estoqueId = null, int? rua = null, int? predio = null, int? nivel = null, bool? lado = null)
        {
            try
            {
                List<Produto> produtos = new List<Produto>();
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();

                    string query = @"
                        SELECT DISTINCT p.id
                        FROM produto p
                        JOIN estoque_produto ep ON p.id = ep.produto_id
                        WHERE p.id > @ID_ATUAL
                        AND (@ESTOQUE_ID IS NULL OR ep.estoque_id = @ESTOQUE_ID)
                        AND (@RUA IS NULL OR ep.rua = @RUA)
                        AND (@PREDIO IS NULL OR ep.predio = @PREDIO)
                        AND (@NIVEL IS NULL OR ep.nivel = @NIVEL)
                        AND (@LADO IS NULL OR ep.lado = @LADO)";
                    if (qtd_retorno > 0) query += $" LIMIT {qtd_retorno}";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID_ATUAL", id_atual);
                        cmd.Parameters.AddWithValue("@QTD_RETORNO", qtd_retorno);
                        cmd.Parameters.AddWithValue("@ESTOQUE_ID", estoqueId.HasValue ? estoqueId.Value : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@RUA", rua.HasValue ? rua.Value : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PREDIO", predio.HasValue ? predio.Value : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@NIVEL", nivel.HasValue ? nivel.Value : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@LADO", lado.HasValue ? lado.Value : (object)DBNull.Value);

                        List<int> idsEncontrados = new List<int>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idsEncontrados.Add(reader.GetInt32("id"));
                            }
                        }

                        foreach (var id in idsEncontrados)
                        {
                            var produto = obterProduto(id);
                            if (produto != null)
                                produtos.Add(produto);
                        }
                    }
                }

                return produtos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar produtos no banco de dados.", ex);
            }
        }

        public Categoria? obterCategoria(int produtoId)
        {
            using (var conn = Conectar())
            {
                conn.Open();
                string query = @"SELECT c.id, c.nome, c.linha
                         FROM categoria c
                         JOIN produto p ON p.categoria_id = c.id
                         WHERE p.id = @PRODUTO_ID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PRODUTO_ID", produtoId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Categoria
                            {
                                Id = reader.GetInt32("id"),
                                Nome = reader.GetString("nome"),
                                Linha = reader.GetString("linha")
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<Categoria> listarCategorias()
        {
            try
            {
                List<Categoria> categorias = new List<Categoria>();

                using (var conn = Conectar())
                {
                    conn.Open();
                    string query = @"SELECT id, nome, linha FROM categoria";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categorias.Add(new Categoria
                                {
                                    Id = reader.GetInt32("id"),
                                    Nome = reader.GetString("nome"),
                                    Linha = reader.GetString("linha")
                                });
                            }
                        }
                    }
                }

                return categorias;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter categorias do banco de dados.", ex);
            }
        }


        public Imagem? obterImagemEstoque(int produtoId)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = @"
                SELECT i.id, i.url
                FROM imagem i
                JOIN imagem_estoque ie ON i.id = ie.imagem_id
                WHERE ie.produto_id = @PRODUTO_ID
                LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PRODUTO_ID", produtoId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Imagem
                                {
                                    Id = reader.GetInt32("id"),
                                    Url = reader.GetString("url")
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter imagem de estoque do produto.", ex);
            }
        }

        public List<Estoque> obterEstoques(int produtoId)
        {
            List<Estoque> estoques = new List<Estoque>();
            using (var conn = Conectar())
            {
                conn.Open();
                string query = @"
            SELECT e.id AS estoque_id, ep.id AS estoque_produto_id, e.nome, ep.rua, ep.predio, ep.nivel, ep.lado, ep.quantidade
            FROM estoque e
            JOIN estoque_produto ep ON e.id = ep.estoque_id
            WHERE ep.produto_id = @PRODUTO_ID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PRODUTO_ID", produtoId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            estoques.Add(new Estoque
                            {
                                ID = reader.GetInt32("estoque_produto_id"),
                                Locais = new LocaisEstoque
                                {
                                    ID = reader.GetInt32("estoque_id"),
                                    Nome = reader.GetString("nome")
                                },
                                Rua = reader.GetInt32("rua"),
                                Predio = reader.GetInt32("predio"),
                                Nivel = reader.GetInt32("nivel"),
                                Lado = reader.GetBoolean("lado"),
                                Quantidade = reader.GetInt32("quantidade")
                            });
                        }
                    }
                }
            }
            return estoques;
        }

        public List<Estoque> listarEstoques()
        {
            try
            {
                List<Estoque> estoques = new List<Estoque>();

                using (var conn = Conectar())
                {
                    conn.Open();
                    string query = @"SELECT * FROM estoque";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                estoques.Add(new Estoque
                                {
                                    Locais = new LocaisEstoque
                                    {
                                        ID = reader.GetInt32("id"),
                                        Nome = reader.GetString("nome")
                                    }
                                });
                            }
                        }
                    }
                }

                return estoques;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter os estoques do banco de dados.", ex);
            }
        }

        public List<Codigo_Adicional> obterCodigoAdicionals(int produtoId)
        {
            List<Codigo_Adicional> codigos = new List<Codigo_Adicional>();
            using (var conn = Conectar())
            {
                conn.Open();
                string query = @"
            SELECT id, codigo
            FROM codigos_adicionais
            WHERE produto_id = @PRODUTO_ID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PRODUTO_ID", produtoId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            codigos.Add(new Codigo_Adicional
                            {
                                Id = reader.GetInt32("id"),
                                Codigo = reader.GetString("codigo")
                            });
                        }
                    }
                }
            }
            return codigos;
        }
        #endregion

        #region Deletar/Modificar Produto
        public void deletarCodigo(Codigo_Adicional codigo)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = @"DELETE FROM codigos_adicionais WHERE id = @CODIGO_ID";
                    MySqlParameter[] parametros = {
                        new MySqlParameter("@CODIGO_ID", codigo.Id)
                    };
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao apagar o codigo no banco de dados.", ex);
            }
        }

        public void deletarEstoque(Estoque estoque)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = @"DELETE FROM estoque_produto WHERE id = @ESTOQUE_ID";
                    MySqlParameter[] parametros = {
                        new MySqlParameter("@ESTOQUE_ID", estoque.ID)
                    };
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao apagar o estoque no banco de dados.", ex);
            }
        }

        public void editarProduto(Produto produto)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();

                    // Atualizar o produto principal
                    string query = @"UPDATE produto 
                             SET descricao = @DESCRICAO, categoria_id = @CATEGORIA, codigo_produto = @CODIGO 
                             WHERE id = @ID";
                    MySqlParameter[] parametros = {
                        new MySqlParameter("@ID", produto.ID),
                        new MySqlParameter("@DESCRICAO", produto.Descricao),
                        new MySqlParameter("@CATEGORIA", produto.Categoria!.Id),
                        new MySqlParameter("@CODIGO", produto.CodigoProduto)
                    };
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        cmd.ExecuteNonQuery();
                    }

                    if (produto.Estoques != null)
                        foreach (var est in produto.Estoques)
                            editarEstoque(est, produto.ID);

                    if (produto.Codigos_Adicionais != null)
                        foreach (var cod in produto.Codigos_Adicionais)
                            editarCodigo(cod, produto.ID);

                    if (produto.Imagem_Estoque != null && produto.Imagem_Estoque.Url != null)
                        editarImagemEstoque(produto.Imagem_Estoque, produto.ID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar o produto.", ex);
            }
        }

        private void editarEstoque(Estoque est, int produtoId)
        {
            if (est.ID == null)
            {
                adicionarProdutoEstoque(est, produtoId);
                return;
            }
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = @"UPDATE estoque_produto 
                     SET rua = @RUA, predio = @PREDIO, nivel = @NIVEL, lado = @LADO, quantidade = @QUANTIDADE 
                     WHERE id = @ID AND produto_id = @PRODUTO_ID";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", est.ID);
                        cmd.Parameters.AddWithValue("@PRODUTO_ID", produtoId);
                        cmd.Parameters.AddWithValue("@RUA", est.Rua);
                        cmd.Parameters.AddWithValue("@PREDIO", est.Predio);
                        cmd.Parameters.AddWithValue("@NIVEL", est.Nivel);
                        cmd.Parameters.AddWithValue("@LADO", est.Lado);
                        cmd.Parameters.AddWithValue("@QUANTIDADE", est.Quantidade);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar o estoque no banco de dados.", ex);
            }

        }

        private void editarCodigo(Codigo_Adicional cod, int produtoId)
        {
            if (cod.Id == null)
            {
                adicionarCodigo(cod, produtoId);
                return;
            }
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = @"UPDATE codigos_adicionais 
                     SET codigo = @CODIGO 
                     WHERE id = @ID AND produto_id = @PRODUTO_ID";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", cod.Id);
                        cmd.Parameters.AddWithValue("@PRODUTO_ID", produtoId);
                        cmd.Parameters.AddWithValue("@CODIGO", cod.Codigo);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar o Codigo no banco de dados.", ex);
            }

        }

        private void editarImagemEstoque(Imagem img, int produtoId)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();

                    // Verifica se já existe uma imagem ligada ao produto
                    string selectQuery = @"SELECT ie.imagem_id 
                                   FROM imagem_estoque ie 
                                   WHERE ie.produto_id = @PRODUTO_ID";

                    using (var selectCmd = new MySqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@PRODUTO_ID", produtoId);
                        var result = selectCmd.ExecuteScalar();

                        if (result != null) // Já existe: apenas atualiza
                        {
                            string updateQuery = @"UPDATE imagem i
                                           JOIN imagem_estoque ie ON i.id = ie.imagem_id
                                           SET i.url = @URL
                                           WHERE ie.produto_id = @PRODUTO_ID";

                            using (var updateCmd = new MySqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@URL", img.Url);
                                updateCmd.Parameters.AddWithValue("@PRODUTO_ID", produtoId);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else // Não existe: reutiliza função de adicionar
                        {
                            conn.Close(); // Fecha antes de chamar sua função
                            adicionarImagemEstoque(img, produtoId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar ou inserir imagem no banco de dados.", ex);
            }
        }
        #endregion

        #endregion

        #region Logs

        //realizar logs mais diversificados futuramente
        //por exemplo, uma função que compara o estoque antes e depois da mudança

        public void LogNovoProduto(int userID, Produto produto)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = @"INSERT INTO log_estoque (produto_id, user_id, contexto, contexto_escrito) 
                             VALUES (@PRODUTO_ID, @USER_ID, @CONTEXTO, @CONTEXTO_ESCRITO)";
                    MySqlParameter[] parametros = {
                        new MySqlParameter("@PRODUTO_ID", produto.ID),
                        new MySqlParameter("@USER_ID", userID),
                        new MySqlParameter("@CONTEXTO", MySqlDbType.Int32) { Value = 0 },
                        new MySqlParameter("@CONTEXTO_ESCRITO", $"Novo Produto {produto.CodigoProduto} inserido.")
                    };
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao guardar informações no banco de dados.", ex);
            }
        }

        public void LogEditarProduto(int userID, Produto produto)
        {
            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = @"INSERT INTO log_estoque (produto_id, user_id, contexto, contexto_escrito) 
                             VALUES (@PRODUTO_ID, @USER_ID, @CONTEXTO, @CONTEXTO_ESCRITO)";
                    MySqlParameter[] parametros = {
                        new MySqlParameter("@PRODUTO_ID", produto.ID),
                        new MySqlParameter("@USER_ID", userID),
                        new MySqlParameter("@CONTEXTO", 1),
                        new MySqlParameter("@CONTEXTO_ESCRITO", $"Produto {produto.CodigoProduto} editado")
                    };
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parametros);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao guardar informações no banco de dados.", ex);
            }
        }

        public List<Log_Produto> TodosLogs(int ultimoId, int qtd, int? tipo)
        {
            List<Log_Produto> logs = new();

            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = @"SELECT le.id, p.codigo_produto, u.email, le.contexto_escrito, DATE(le.data_registro) as registro
                             FROM log_estoque le
                             INNER JOIN produto p ON le.produto_id = p.id
                             INNER JOIN usuario u ON le.user_id = u.id
                             WHERE le.id > @ULTIMO_ID";

                    if (tipo.HasValue)
                        query += " AND le.contexto = @TIPO";

                    query += " ORDER BY le.registro DESC";

                    if (qtd > 0)
                        query += " LIMIT @QTD";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ULTIMO_ID", ultimoId);
                        if (tipo.HasValue)
                            cmd.Parameters.AddWithValue("@TIPO", tipo.Value);
                        if (qtd > 0)
                            cmd.Parameters.AddWithValue("@QTD", qtd);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new Log_Produto
                                {
                                    ID = reader.GetInt32("id"),
                                    CodigoProduto = reader.GetString("codigo_produto"),
                                    EmailUser = reader.GetString("email"),
                                    ContextoEscrito = reader.GetString("contexto_escrito"),
                                    Registro = DateOnly.FromDateTime(reader.GetDateTime("registro"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar logs de produto.", ex);
            }

            return logs;
        }


        public List<Log_Produto> BuscarLogs(int ultimoId, int qtd, string termo, int? tipo)
        {
            List<Log_Produto> logs = new();

            try
            {
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = @"SELECT le.id, p.codigo_produto, u.email, le.contexto_escrito, DATE(le.registro) as registro
                             FROM log_estoque le
                             INNER JOIN produto p ON le.produto_id = p.id
                             INNER JOIN usuario u ON le.user_id = u.id
                             WHERE le.id > @ULTIMO_ID
                               AND (p.codigo_produto LIKE @TERMO OR u.email LIKE @TERMO)";

                    if (tipo.HasValue)
                        query += " AND le.contexto = @TIPO";

                    query += " ORDER BY le.registro DESC";

                    if (qtd > 0)
                        query += " LIMIT @QTD";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ULTIMO_ID", ultimoId);
                        cmd.Parameters.AddWithValue("@TERMO", $"%{termo}%");
                        if (tipo.HasValue)
                            cmd.Parameters.AddWithValue("@TIPO", tipo.Value);
                        if (qtd > 0)
                            cmd.Parameters.AddWithValue("@QTD", qtd);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new Log_Produto
                                {
                                    ID = reader.GetInt32("id"),
                                    CodigoProduto = reader.GetString("codigo_produto"),
                                    EmailUser = reader.GetString("email"),
                                    ContextoEscrito = reader.GetString("contexto_escrito"),
                                    Registro = DateOnly.FromDateTime(reader.GetDateTime("registro"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar logs com termo.", ex);
            }

            return logs;
        }


        #endregion

        #region funções para a loja virtual

        public void adicionarImagemLoja(Imagens_Loja imgLoja, int produtoId)
        {
            using (var conn = Conectar())
            {
                conn.Open();

                // Inserir imagem
                string insertImagem = "INSERT INTO imagem (url) VALUES (@URL)";
                using (var cmd = new MySqlCommand(insertImagem, conn))
                {
                    cmd.Parameters.AddWithValue("@URL", imgLoja.Imagem!.Url);
                    cmd.ExecuteNonQuery();
                }

                // Obter ID da imagem
                int imagemId;
                using (var cmd = new MySqlCommand("SELECT LAST_INSERT_ID()", conn))
                {
                    imagemId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Inserir vínculo imagem_loja
                string insertVinculo = "INSERT INTO imagem_loja (produto_id, imagem_id, ordem) VALUES (@PRODUTO_ID, @IMAGEM_ID, @ORDEM)";
                using (var cmd = new MySqlCommand(insertVinculo, conn))
                {
                    cmd.Parameters.AddWithValue("@PRODUTO_ID", produtoId);
                    cmd.Parameters.AddWithValue("@IMAGEM_ID", imagemId);
                    cmd.Parameters.AddWithValue("@ORDEM", imgLoja.Ordem);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Imagens_Loja> obterImagensLoja(int produtoId)
        {
            try
            {
                List<Imagens_Loja> imagensLoja = new List<Imagens_Loja>();
                using (MySqlConnection conn = Conectar())
                {
                    conn.Open();
                    string query = @"
                SELECT il.ordem, i.id, i.url
                FROM imagem i
                JOIN imagem_loja il ON i.id = il.imagem_id
                WHERE il.produto_id = @PRODUTO_ID
                ORDER BY il.ordem ASC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PRODUTO_ID", produtoId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                imagensLoja.Add(new Imagens_Loja
                                {
                                    Ordem = reader.GetInt32("ordem"),
                                    Imagem = new Imagem
                                    {
                                        Id = reader.GetInt32("id"),
                                        Url = reader.GetString("url")
                                    }
                                });
                            }
                        }
                    }
                }
                return imagensLoja;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter imagens da loja do produto.", ex);
            }
        }

        #endregion
    }
}
