# Solar Power Plant Monitoring API

Ova aplikacija omogućuje kreiranje i upravljanje solarnim elektranama, kao i dohvaćanje produktivnih podataka temeljenih na vremenskim uvjetima. Aplikacija također sadrži funkcionalnost za registraciju i prijavu korisnika, gdje se nakon uspješne prijave generira JWT token za autorizaciju.

### Glavne funkcionalnosti:
- **Registracija i prijava korisnika**
- **CRUD operacije (Create, Read, Update, Delete)** za solarne elektrane
- **Dohvaćanje proizvodnih podataka** na temelju vremenskih uvjeta (s OpenWeather API-jem)
- **Autorizacija pomoću JWT tokena** za pristup zaštićenim API endpointima
- ** logovi 
<img width="1440" alt="Slika zaslona 2024-12-02 u 16 14 30" src="https://github.com/user-attachments/assets/25af9adc-1227-4fbb-b0fe-d6b1c16a418a">

Primjer u responsu
<img width="850" alt="Slika zaslona 2024-12-02 u 14 04 25" src="https://github.com/user-attachments/assets/fe452c02-392a-4da0-a264-1c5d6a3c2d4a">
<img width="809" alt="Slika zaslona 2024-12-02 u 14 04 32" src="https://github.com/user-attachments/assets/b04d3ee5-7384-4979-b086-91673322c483">


### Napomena:
- Prilikom dodavanja solarne elektrane, aplikacija automatski izračunava prognozu proizvodnje temeljem vremenskih uvjeta dohvaćenih s **OpenWeather API**. 
- Zbog testiranja i praktičnosti iako sam napisao logiku, i iako nije specificirano u zadatku, stavio sam da se produkcijski podaci za svaku elektranu  dohvaćaju **svaku minutu**, neovisno o broju elektrana. Granularnost je izmjenjena zbog testiranja,  naveo sam komentare u Production Service komponenti i logiku za granularnost, gdje bi se resetala ovisno o parametrima, u slučaju zadatka sat vremena. 
- Pristupanje endpointima, kao što je navedeno zaštićeno je jwt tokenom tako da ga nemojte zaboraviti postaviti  u headeru za autorizaciju preko postmana
## Instalacija i Pokretanje Aplikacije

1. **Klonirajte projekt** na svoje računalo.
2. Otvorite projekt u Visual Studio.
3. Koristite dotnet restore kako bi dohvatili sve pakete koje projekt sadrži.
4. Koristio sam Sqlite( na macu)  potrebno je izvršiti migracije za bazu. dotnet ef migrations add InitialCreate
5. Zatim pokrenite projekt s dotnet run.


### API Endpoints
#### 1. **Registracija korisnika**
   - **POST**: `POST api/Auth/register`
   - **Opis**: Registrira novog korisnika.

   **Primjer zahtjeva**:
   ```json
   POST /api/Auth/register
   Content-Type: application/json
   {
     "username": "newuser",
     "password": "password123"
   }

Postman
#### 2. **Dodavanje solarne elektrane**
   - **PUT**: `POST api/SolarPowerPlant/addSolarPowerPlant`
   - **Opis**: Registrira novu solarnu elektranu. Prilikom dodavanja, izračunava se produktivnost na temelju vremenskih uvjeta.
   - **Primjer zahtjeva**:
     ```json
     POST /api/SolarPowerPlant/addSolarPowerPlant
     Content-Type: application/json
     Authorization: Bearer <your_jwt_token>
     {
       "name": "Solar Plant 1",
       "installedPower": 100,
       "dateOfInstallation": "2023-01-01",
       "latitude": 45.12345,
       "longitude": 15.12345
     }
     ```

#### 2. **Ažuriranje solarne elektrane**
   - **PUT**: `PUT api/SolarPowerPlant/updateSolarPowerPlant/{id}`
   - **Opis**: Ažurira podatke o solarnoj elektrani s danim ID-jem.

#### 3. **Brisanje solarne elektrane**
   - **DELETE**: `DELETE api/SolarPowerPlant/deleteSolarPowerPlant/{id}`
   - **Opis**: Briše solarnu elektranu s danim ID-jem.

#### 4. **Dobavljanje solarnih elektrana**
   - **GET**: `GET api/SolarPowerPlant/getSolarPowerPlants`
   - **Opis**: Dohvaća sve solarne elektrane povezane s korisnikom.

### Putanje za ProductionController

http://localhost:5032/api/Production/{id}
U body response biti će vam ispisani svi produkcijski zapisi elektrane s obzirom na  vrijeme.

Ukratko, u projektu se koristi weather background service koji prilikom podizanja projekta šalje zahtjev svake minute na weather service koji opet poziva api. Production service računa produktivnost utemljenu na vremenskim podatcima i background servis sve to sprema prije nego što i njega pokrene. Također tu su modalsi, controlleri i ostale stvari koje su složene u projektu.


