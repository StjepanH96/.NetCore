# Solar Power Plant Monitoring API

Ova aplikacija omogućuje kreiranje i upravljanje solarnim elektranama, kao i dohvaćanje produktivnih podataka temeljenih na vremenskim uvjetima. Aplikacija također sadrži funkcionalnost za registraciju i prijavu korisnika, gdje se nakon uspješne prijave generira JWT token za autorizaciju.

### Glavne funkcionalnosti:
- **Registracija i prijava korisnika**
- **CRUD operacije (Create, Read, Update, Delete)** za solarne elektrane
- **Dohvaćanje proizvodnih podataka** na temelju vremenskih uvjeta (s OpenWeather API-jem)
- **Autorizacija pomoću JWT tokena** za pristup zaštićenim API endpointima
- ** logovi 

Primjer responsa i  loga  samo za test, granularnost jedna minuta, 4 minute zapis. Ukupna produktivnost za dvije elektrane, u logovima se zapisuje pojedinačna isto tako kao što je vidljivo 
![Slika zaslona 2024-12-02 u 18 27 21 (2)](https://github.com/user-attachments/assets/064d6942-fa76-4497-8ceb-99be57f412ed)
<img width="1440" alt="Slika zaslona 2024-12-02 u 18 27 05" src="https://github.com/user-attachments/assets/309e414f-5a29-4854-9843-fb34fe6f7f8b">
<img width="888" alt="Slika zaslona 2024-12-04 u 12 30 28" src="https://github.com/user-attachments/assets/90302535-c1c0-4aa1-bfd2-5bedd8a7d93c">




### Napomena:
- Prilikom dodavanja solarne elektrane, aplikacija automatski izračunava prognozu proizvodnje temeljem vremenskih uvjeta dohvaćenih s **OpenWeather API**. 
- Svakih petnaest minuta će se dodati zapis, granularnost je konfigurirana da nakon četiri zapisa, doda se 1 hour zapis koji će izračunati ukupnu produktivnost elektrane u tom periodu od 4 zapisa.
- Ako korisnik ima više od jedne elektrane, za svaku elektranu će se zapisivati zasebno. Slika iznad
- Pristupanje endpointima, kao što je navedeno zaštićeno je jwt tokenom. Postavite ga u header.
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

   - #### 2. **Prijava korisnika**
   - **POST**: `POST api/Auth/login`
   - **Opis**: Prijava novog korisnika.
   - dobivanje tokena
         
   **Primjer zahtjeva za login i register**:
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

Ukratko, u projektu se koristi weather background service koji prilikom podizanja projekta šalje zahtjev svakih petnaest miuta na weather service koji opet poziva api. Production service računa produktivnost utemljenu na vremenskim podatcima.Također tu su modalsi, controlleri i ostale stvari koje su složene u projektu.


