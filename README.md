# 📅 AgendaUni

Organize sua vida acadêmica com praticidade! O AgendaUni é um aplicativo mobile simples e intuitivo que ajuda estudantes a gerenciar provas, faltas, trabalhos e compromissos da faculdade, tudo em um só lugar, mesmo sem internet.

# 🚀 Funcionalidades

- [x] Cadastro de provas, faltas e trabalhos

- [x] Gerenciamento de datas importantes

- [x] Funciona offline

- [ ]  Integração futura com API e versão web

# 📷 Screenshots
<img width="385" height="548" alt="image" src="https://github.com/user-attachments/assets/ea9a9bb9-1a87-48c6-9958-841429370ed6" /> <br>

<img width="384" height="334" alt="image" src="https://github.com/user-attachments/assets/3feffc9f-4e0c-4dde-b4ee-38b943e34661" /> <br>

<img width="387" height="403" alt="image" src="https://github.com/user-attachments/assets/8c77de10-992e-4227-8398-0add91ac11d2" /> <br>

<img width="384" height="403" alt="image" src="https://github.com/user-attachments/assets/17427d16-6875-4ace-8531-45c888231e6c" /> <br>

<img width="383" height="410" alt="image" src="https://github.com/user-attachments/assets/9ef27ced-9921-47ad-9749-2ead516cb32c" />


# 🔧 Instalação
### Pré-requisitos

.NET MAUI instalado

Visual Studio 2022 (ou superior) configurado com suporte a .NET MAUI

Emulador Android ou dispositivo físico conectado

## Rodando o projeto

### Clone o repositório
git clone https://github.com/BrenoTrindade/AgendaUni.git

### Acesse a pasta do projeto
cd AgendaUni

### Restaure as dependências
dotnet restore

### Acesse a pasta da API e crie a migração inicial

dotnet ef migrations add InitialCreate

### Aplicar a migração ao banco de dados:
dotnet ef database update

### Execute no emulador ou dispositivo
dotnet build

dotnet run

# 📦 Publicação

O aplicativo será publicado na Play Store em breve.

# 🛠️ Tecnologias utilizadas

.NET MAUI

C#

SQLite (armazenamento local offline)

# 📜 Licença
Este projeto é de código aberto sob a licença MIT License para fins de portfólio. Para uso comercial ou distribuição, entre em contato com o desenvolvedor.
