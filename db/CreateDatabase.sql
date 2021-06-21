CREATE DATABASE IF NOT EXISTS dev;

USE dev;

CREATE TABLE IF NOT EXISTS dev.user (
  id INT NOT NULL AUTO_INCREMENT,
  isActive TINYINT(1) NOT NULL DEFAULT '0',
  createdDate DATETIME NOT NULL,
  userName VARCHAR(32) NOT NULL,
  password VARCHAR(32) NOT NULL,
  realName VARCHAR(32) NOT NULL,
  numCpf VARCHAR(11) NOT NULL,
  email VARCHAR(150) NOT NULL,
  phone VARCHAR(13) NULL DEFAULT NULL,
  PRIMARY KEY (id),
  UNIQUE INDEX email (email ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 2;

CREATE TABLE IF NOT EXISTS dev.endereco (
  idEndereco INT NOT NULL AUTO_INCREMENT,
  numCep VARCHAR(8) NOT NULL,
  dsLogradouro VARCHAR(100) NOT NULL,
  dsbairro VARCHAR(80) NOT NULL,
  dsCidade VARCHAR(100) NOT NULL,
  dsEstado VARCHAR(2) NOT NULL,
  dsNumero VARCHAR(6) NULL,
  userId INT NOT NULL,
  isAtivo TINYINT(1) NOT NULL,
  PRIMARY KEY (idEndereco),
  INDEX fk_endereco_user1_idx (userId ASC) VISIBLE,
  CONSTRAINT fk_endereco_user1
    FOREIGN KEY (userId)
    REFERENCES dev.user (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS dev.extrato (
  idExtrato INT NOT NULL AUTO_INCREMENT,
  userId INT NOT NULL,
  dtExtrato DATETIME NOT NULL,
  flTipo VARCHAR(6) NOT NULL COMMENT 'DEBIT OR CREDIT',
  vlMovimentado DECIMAL(11,2) NOT NULL,
  PRIMARY KEY (idExtrato),
  INDEX fk_extrato_user_idx (userId ASC) VISIBLE,
  INDEX ix_userId (userId ASC) VISIBLE,
  INDEX ix_flTipo (flTipo ASC) VISIBLE,
  CONSTRAINT fk_extrato_user
    FOREIGN KEY (userId)
    REFERENCES dev.user (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS dev.categoria (
  idCategoria INT NOT NULL AUTO_INCREMENT,
  nome VARCHAR(45) NOT NULL,
  PRIMARY KEY (idCategoria))
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS dev.subcategoria (
  idSubcategoria INT NOT NULL AUTO_INCREMENT,
  nome VARCHAR(100) NOT NULL,
  categoriaIdCategoria INT NOT NULL,
  PRIMARY KEY (idSubcategoria),
  INDEX fk_subcategoria_categoria1_idx (categoriaIdCategoria ASC) VISIBLE,
  CONSTRAINT fk_subcategoria_categoria1
    FOREIGN KEY (categoriaIdCategoria)
    REFERENCES dev.categoria (idCategoria)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS dev.produto (
  idProduto INT NOT NULL AUTO_INCREMENT,
  nome VARCHAR(100) NOT NULL,
  valorUnitario DECIMAL(11,2) NOT NULL,
  estoque INT NOT NULL,
  subcategoriaIdSubcategoria INT NOT NULL,
  PRIMARY KEY (idProduto),
  INDEX fk_produto_subcategoria1_idx (subcategoriaIdSubcategoria ASC) VISIBLE,
  CONSTRAINT fk_produto_subcategoria1
    FOREIGN KEY (subcategoriaIdSubcategoria)
    REFERENCES dev.subcategoria (idSubcategoria)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS dev.pedido (
  idPedido INT NOT NULL AUTO_INCREMENT,
  dataPedido DATETIME NOT NULL,
  produtoIdProduto INT NOT NULL,
  quantidadeProduto VARCHAR(45) NOT NULL,
  userId INT NOT NULL,
  flStatus TINYINT(1) NOT NULL,
  dataEntrega DATETIME NULL,
  enderecoIdEndereco INT NOT NULL,
  PRIMARY KEY (idPedido),
  INDEX fk_resgate_produto1_idx (produtoIdProduto ASC) VISIBLE,
  INDEX fk_resgate_user1_idx (userId ASC) VISIBLE,
  INDEX fk_pedido_endereco1_idx (enderecoIdEndereco ASC) VISIBLE,
  CONSTRAINT fk_resgate_produto1
    FOREIGN KEY (produtoIdProduto)
    REFERENCES dev.produto (idProduto)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_resgate_user1
    FOREIGN KEY (userId)
    REFERENCES dev.user (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_pedido_endereco1
    FOREIGN KEY (enderecoIdEndereco)
    REFERENCES dev.endereco (idEndereco)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;




SET character_set_client = utf8;
SET character_set_connection = utf8;
SET character_set_results = utf8;
SET collation_connection = utf8_general_ci;

INSERT INTO dev.user (id, isActive, createdDate, userName, password, realName, numCpf, email, phone) VALUES
( 1, 1, NOW(), 'jfulano', '@A$b%c','João Fulano','01822255588','teste@teste.com.br','5579996027329');

INSERT INTO dev.endereco (numCep, dsLogradouro, dsbairro, dsCidade, dsEstado, dsNumero, userId, isAtivo) VALUES
('49160000', 'Av 1', 'Distante', 'Felizcidade', 'SE', '34', 1, 1);

INSERT INTO dev.endereco (numCep, dsLogradouro, dsbairro, dsCidade, dsEstado, dsNumero, userId, isAtivo) VALUES
('49160000', 'Rua 4', 'Distante', 'Felizcidade', 'SE', '107', 1, 0);


INSERT INTO dev.user (id, isActive, createdDate, userName, password, realName, numCpf, email, phone) VALUES
(2, 0, NOW(), 'mfulano', '@A$b%c','Maria Fulano','03522255588','teste2@teste.com.br','5579996027329');

INSERT INTO dev.endereco (numCep, dsLogradouro, dsbairro, dsCidade, dsEstado, dsNumero, userId, isAtivo) VALUES
('49160000', 'Av 5', 'Distante', 'Felizcidade', 'SE', '760', 2, 1);

INSERT INTO dev.endereco (numCep, dsLogradouro, dsbairro, dsCidade, dsEstado, dsNumero, userId, isAtivo) VALUES
('49160000', 'Rua 410', 'Distante', 'Felizcidade', 'SE', '987', 2, 0);

INSERT INTO dev.extrato (userId, dtExtrato, flTipo, vlMovimentado) VALUES
(1, NOW(), 'CREDIT', 100.00);

INSERT INTO dev.extrato (userId, dtExtrato, flTipo, vlMovimentado) VALUES
(1, NOW(), 'CREDIT', 50.00);

INSERT INTO dev.extrato (userId, dtExtrato, flTipo, vlMovimentado) VALUES
(2, NOW(), 'CREDIT', 10.00);

INSERT INTO dev.extrato (userId, dtExtrato, flTipo, vlMovimentado) VALUES
(1, NOW(), 'DEBIT', 30.00);


INSERT INTO categoria (idCategoria, nome) VALUES
(1, 'Carros, peças e Acessórios'),
(2, 'Casa e Escritório'),
(3, 'Restaurantes, Alimentos e Bebidas'),
(4, 'Diversão, Lazer, Entretenimento'),
(5, 'Educação, Cultura, Meios de Comunicação'),
(6, 'Informática e Serviços Gráficos'),
(7, 'Moda e Beleza'),
(8, 'Animais de Estimação e Pet Shops'),
(9, 'Clínicas, Hospitais e Consultórios Médicos');

INSERT INTO subcategoria (idSubcategoria, categoriaIdCategoria, nome) VALUES
(1, 1, 'Aluguel de automóveis'),
(2, 1, 'Pneus'),
(3, 1, 'Carretas'),
(4, 1, 'Consórcios'),
(5, 1, 'Engates'),
(6, 1, 'Vidro de Segurança'),
(7, 1, 'Embreagens'),
(8, 1, 'Filtro de Ar'),
(9, 1, 'Martelinho de Ouro'),
(10, 2, 'Assessoria do Lar'),
(11, 2, 'Avaliadores'),
(12, 2, 'Corretor de Imóveis'),
(13, 2, 'Imobiliárias'),
(14, 2, 'Conserto de Fogões'),
(15, 2, 'Almofadas'),
(16, 2, 'Piscinas'),
(17, 2, 'Grama'),
(18, 3, 'Açougues'),
(19, 3, 'Castanhas'),
(20, 3, 'Cereais'),
(21, 3, 'Frutos do Mar'),
(22, 3, 'Laticínios'),
(23, 3, 'Mel'),
(24, 3, 'Bares e Café'),
(25, 3, 'Cyber Café'),
(26, 4, 'Agências de Casamento'),
(27, 4, 'Artigos de Carnaval'),
(28, 4, 'Cinemas'),
(29, 4, 'Games'),
(30, 4, 'Boliche'),
(31, 4, 'Museus'),
(32, 4, 'Teatros'),
(33, 4, 'Músicos'),
(34, 5, 'Curso de Artesanato'),
(35, 5, 'Escola de Futebol'),
(36, 5, 'Cursos Livres'),
(37, 5, 'Aluguel de Livros'),
(38, 5, 'Bibliotecas'),
(39, 5, 'Uniformes'),
(40, 6, 'Impressoras'),
(41, 6, 'Notebooks'),
(42, 6, 'Projetores'),
(43, 6, 'VOIP'),
(44, 6, 'Acabamentos Gráficos'),
(45, 6, 'Artigos de Carnaval'),
(46, 6, 'Impressão Digital'),
(47, 7, 'Clínicas de Estética'),
(48, 7, 'Bronzeamento Artificial'),
(49, 7, 'Fraldas'),
(50, 7, 'Botas'),
(51, 7, 'Bonés'),
(52, 7, 'Camisetas'),
(53, 7, 'Loja de Malhas'),
(54, 8, 'Canis'),
(55, 8, 'Acupuntura em animais'),
(56, 8, 'Veterinárias'),
(57, 8, 'Pet Shop'),
(58, 8, 'Rações'),
(59, 8, 'Taxi Dog'),
(60, 8, 'Passeadores de cães'),
(61, 9, 'Clínica de Massagem'),
(62, 9, 'Clínica de Psicologia'),
(63, 9, 'Clínica de Reabilitação'),
(64, 9, 'Ultra-Som'),
(65, 9, 'Farmácias e Drogarias'),
(66, 9, 'Lenstes de Contato'),
(67, 9, 'Dentistas');

