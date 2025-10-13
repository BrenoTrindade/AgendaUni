# 📅 AgendaUni

Organize sua vida acadêmica com praticidade! O AgendaUni é um aplicativo mobile simples e intuitivo que ajuda estudantes a gerenciar provas, faltas, trabalhos e compromissos da faculdade, tudo em um só lugar, mesmo sem internet.

# 🚀 Funcionalidades

- [x] Cadastro de provas, faltas e trabalhos

- [x] Gerenciamento de datas importantes

- [x] Funciona offline

- [x] Integração com API

- [ ] Versão web

# 📷 Screenshots
<img width="381" height="552" alt="image" src="https://github.com/user-attachments/assets/fe6fa44f-4530-4dce-ae08-9d2aa6015f2c" /> <br>

<img width="384" height="389" alt="image" src="https://github.com/user-attachments/assets/4e52e278-485f-433b-8b5a-a181979b9b7c" /> <br>

<img width="387" height="786" alt="image" src="https://github.com/user-attachments/assets/077184c0-71f3-417b-85de-fabaaa09b638" /> <br>

<img width="382" height="312" alt="image" src="https://github.com/user-attachments/assets/d3d9de06-d530-4c7a-be83-3ccf4b61e1c8" /> <br>

<img width="385" height="436" alt="image" src="https://github.com/user-attachments/assets/9ffa66c5-89d1-4a05-8374-1ce56a1ed4f2" /> <br>

<img width="382" height="511" alt="image" src="https://github.com/user-attachments/assets/fc6b534c-79a0-4332-920e-818acabbeb44" /> <br>

<img width="385" height="432" alt="image" src="https://github.com/user-attachments/assets/4c1ed0cf-6a6d-4050-99d7-dce9fe08633c" />


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

O AgendaUni está atualmente em **fase de teste interno** na Google Play Store. Apenas testadores convidados têm acesso ao aplicativo neste momento.  

Se você quiser participar dos testes, entre em contato com o desenvolvedor para receber o convite.  
Em breve, o aplicativo estará disponível para todos os usuários na versão oficial.

# 🛠️ Tecnologias utilizadas

.NET MAUI

BLAZOR

SQLSERVER

C#

SQLite (armazenamento local offline)

XUNIT

# 📜 Licença
Este projeto é de código aberto sob a licença MIT License para fins de portfólio. Para uso comercial ou distribuição, entre em contato com o desenvolvedor.
