create table usuario
(
id serial Primary key,
usuario varchar (50),
contrasena varchar (50)
)

create table tipo_mensage
(
id serial Primary key,
descripcion varchar (50)
)


create table mensaje
(
id serial primary key,
descripcion varchar(200),
link varchar(200),
id_usuario int ,
id_tipo_mensage int,
foreign key (id_usuario) references usuario(id),
foreign key (id_tipo_mensage) references tipo_mensage(id)
)
