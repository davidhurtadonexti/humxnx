Declare @clientId UNIQUEIDENTIFIER;
Declare @clientServiceMobilId UNIQUEIDENTIFIER;
Declare @clientServiceWebId UNIQUEIDENTIFIER;
Declare @loginServiceId UNIQUEIDENTIFIER;
Declare @brokerServiceId UNIQUEIDENTIFIER;

--CLIENTS
insert into Clients 
(Descripcion) 
values 
('Servicios Ambulatorios')
,('Dentales')
,('Farmacias')
,('Servicios Hospitalarios')
,('Pago Prestadores')
,('Brokers')

select @clientId =Id from Clients where Descripcion = 'Brokers'


--CLIENT SERVICES
insert into ClientServices
(ClientId, PublicKey, SecretKey, Descripcion, ServiceType, Hosts, AccesTokenExp, RefreshTokenExp)
values
(@clientId, 'humana_broker_key','the_secret_key','Brocker Portal Web','web','www.broker.com',5,10)
,(@clientId, 'humana_broker_key_mobil','the_secret_key_mobil','Brocker Portal Mobil','mobil','www.md_broker.com',5,10)

select @clientServiceMobilId =Id from ClientServices where PublicKey = 'humana_broker_key_mobil'
select @clientServiceWebId =Id from ClientServices where PublicKey = 'humana_broker_key'

--RESOURCES
insert into Resources
(Descripcion, ResourceUrl)
values
('Login Service','http://localhost:8080/api/login')
,('Brocker Service','http://localhost:8081/api/Broker')

select @loginServiceId =Id from Resources where Descripcion = 'Login Service'
select @brokerServiceId =Id from Resources where Descripcion = 'Broker Service'


--SESSION PERMISSIONS
insert into SessionPermissions
(ClientServiceId, ResourceId, ScopePermissions)
values
--broker
(@clientServiceWebId,@brokerServiceId,'rwud') --broker service
,(@clientServiceWebId,@loginServiceId,'r---') --login
--broker mobil
,(@clientServiceMobilId,@brokerServiceId,'rw--') --broker service
,(@clientServiceMobilId,@loginServiceId,'r---') --login
