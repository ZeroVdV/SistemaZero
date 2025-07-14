-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 14/07/2025 às 22:02
-- Versão do servidor: 10.4.32-MariaDB
-- Versão do PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Banco de dados: `bd_zero`
--

-- --------------------------------------------------------

--
-- Estrutura para tabela `categoria`
--

CREATE TABLE `categoria` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `linha` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `codigos_adicionais`
--

CREATE TABLE `codigos_adicionais` (
  `id` int(11) NOT NULL,
  `produto_id` int(11) NOT NULL,
  `codigo` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `estoque`
--

CREATE TABLE `estoque` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `estoque_produto`
--

CREATE TABLE `estoque_produto` (
  `id` int(11) NOT NULL,
  `produto_id` int(11) NOT NULL,
  `estoque_id` int(11) NOT NULL,
  `rua` int(11) DEFAULT NULL,
  `predio` int(11) DEFAULT NULL,
  `nivel` int(11) DEFAULT NULL,
  `lado` tinyint(1) DEFAULT NULL,
  `quantidade` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `imagem`
--

CREATE TABLE `imagem` (
  `id` int(11) NOT NULL,
  `url` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `imagem_estoque`
--

CREATE TABLE `imagem_estoque` (
  `produto_id` int(11) NOT NULL,
  `imagem_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `imagem_loja`
--

CREATE TABLE `imagem_loja` (
  `produto_id` int(11) NOT NULL,
  `imagem_id` int(11) NOT NULL,
  `ordem` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `log_estoque`
--

CREATE TABLE `log_estoque` (
  `id` int(11) NOT NULL,
  `produto_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `contexto` int(11) NOT NULL COMMENT '0 - adição\r\n1 - edição\r\n2 - edição puramente do estoque\r\n',
  `contexto_escrito` text NOT NULL,
  `registro` date NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `produto`
--

CREATE TABLE `produto` (
  `id` int(11) NOT NULL,
  `descricao` varchar(200) NOT NULL,
  `categoria_id` int(11) DEFAULT NULL,
  `codigo_produto` varchar(100) DEFAULT NULL,
  `ativo` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `usuario`
--

CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `senha` varchar(100) NOT NULL,
  `cargo` int(11) NOT NULL,
  `ativo` tinyint(1) DEFAULT 1,
  `registro` date DEFAULT curdate()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `usuario`
--

INSERT INTO `usuario` (`id`, `nome`, `email`, `senha`, `cargo`, `ativo`, `registro`) VALUES
(1, 'Administrador', 'adm@gmail.com', 'lsrjXOipsCRBeL8o5JZsLOG4OFcjqWprg4hYzdbKCh4=', 0, 1, '2025-06-24'),
(3, 'Usuario Comum', 'user@gmail.com', 'lsrjXOipsCRBeL8o5JZsLOG4OFcjqWprg4hYzdbKCh4=', 1, 1, '2025-07-08');

--
-- Índices para tabelas despejadas
--

--
-- Índices de tabela `categoria`
--
ALTER TABLE `categoria`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `codigos_adicionais`
--
ALTER TABLE `codigos_adicionais`
  ADD PRIMARY KEY (`id`),
  ADD KEY `produto_id` (`produto_id`);

--
-- Índices de tabela `estoque`
--
ALTER TABLE `estoque`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `estoque_produto`
--
ALTER TABLE `estoque_produto`
  ADD PRIMARY KEY (`id`),
  ADD KEY `produto_id` (`produto_id`),
  ADD KEY `estoque_id` (`estoque_id`);

--
-- Índices de tabela `imagem`
--
ALTER TABLE `imagem`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `imagem_estoque`
--
ALTER TABLE `imagem_estoque`
  ADD PRIMARY KEY (`produto_id`,`imagem_id`),
  ADD KEY `imagem_id` (`imagem_id`);

--
-- Índices de tabela `imagem_loja`
--
ALTER TABLE `imagem_loja`
  ADD PRIMARY KEY (`produto_id`,`imagem_id`),
  ADD KEY `imagem_id` (`imagem_id`);

--
-- Índices de tabela `log_estoque`
--
ALTER TABLE `log_estoque`
  ADD PRIMARY KEY (`id`),
  ADD KEY `produto_id` (`produto_id`),
  ADD KEY `user_id` (`user_id`);

--
-- Índices de tabela `produto`
--
ALTER TABLE `produto`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `codigo_loid` (`codigo_produto`),
  ADD KEY `categoria_id` (`categoria_id`);

--
-- Índices de tabela `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- AUTO_INCREMENT para tabelas despejadas
--

--
-- AUTO_INCREMENT de tabela `categoria`
--
ALTER TABLE `categoria`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `codigos_adicionais`
--
ALTER TABLE `codigos_adicionais`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `estoque`
--
ALTER TABLE `estoque`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `estoque_produto`
--
ALTER TABLE `estoque_produto`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `imagem`
--
ALTER TABLE `imagem`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `log_estoque`
--
ALTER TABLE `log_estoque`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `produto`
--
ALTER TABLE `produto`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Restrições para tabelas despejadas
--

--
-- Restrições para tabelas `codigos_adicionais`
--
ALTER TABLE `codigos_adicionais`
  ADD CONSTRAINT `codigos_adicionais_ibfk_1` FOREIGN KEY (`produto_id`) REFERENCES `produto` (`id`) ON DELETE CASCADE;

--
-- Restrições para tabelas `estoque_produto`
--
ALTER TABLE `estoque_produto`
  ADD CONSTRAINT `estoque_produto_ibfk_1` FOREIGN KEY (`produto_id`) REFERENCES `produto` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `estoque_produto_ibfk_2` FOREIGN KEY (`estoque_id`) REFERENCES `estoque` (`id`) ON DELETE CASCADE;

--
-- Restrições para tabelas `imagem_estoque`
--
ALTER TABLE `imagem_estoque`
  ADD CONSTRAINT `imagem_estoque_ibfk_1` FOREIGN KEY (`produto_id`) REFERENCES `produto` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `imagem_estoque_ibfk_2` FOREIGN KEY (`imagem_id`) REFERENCES `imagem` (`id`) ON DELETE CASCADE;

--
-- Restrições para tabelas `imagem_loja`
--
ALTER TABLE `imagem_loja`
  ADD CONSTRAINT `imagem_loja_ibfk_1` FOREIGN KEY (`produto_id`) REFERENCES `produto` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `imagem_loja_ibfk_2` FOREIGN KEY (`imagem_id`) REFERENCES `imagem` (`id`) ON DELETE CASCADE;

--
-- Restrições para tabelas `log_estoque`
--
ALTER TABLE `log_estoque`
  ADD CONSTRAINT `log_estoque_ibfk_1` FOREIGN KEY (`produto_id`) REFERENCES `produto` (`id`),
  ADD CONSTRAINT `log_estoque_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `usuario` (`id`);

--
-- Restrições para tabelas `produto`
--
ALTER TABLE `produto`
  ADD CONSTRAINT `produto_ibfk_1` FOREIGN KEY (`categoria_id`) REFERENCES `categoria` (`id`) ON DELETE SET NULL;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
