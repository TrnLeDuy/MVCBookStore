USE master
GO
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'bookstoreDB')
BEGIN
	DROP DATABASE bookstoreDB
END
CREATE DATABASE bookstoreDB
GO

USE bookstoreDB
GO

CREATE TABLE CHUDE
(
	MaCD int identity (1,1) PRIMARY KEY,
	TenChuDe nvarchar(50) NOT NULL,
);

CREATE TABLE NHAXUATBAN
(
	MaNXB int identity (1,1) PRIMARY KEY,
	TenNXB nvarchar(100) NOT NULL,
	Diachi nvarchar(150),
	DienThoai nvarchar(15),
);

CREATE TABLE SACH
(
	Masach int identity (1,1) PRIMARY KEY,
	Tensach nvarchar(100) NOT NULL,
	Donvitinh nvarchar(50) default 'cuốn',
	Dongia MONEY CHECK (Dongia >= 0),
	Mota ntext,
	Hinhminhhoa varchar(50),
	MaCD int,
	MaNXB int,
	Ngaycapnhat smalldatetime,
	Soluongban int check (Soluongban > 0),
	solanxem int default 0,
	FOREIGN KEY (MaCD) REFERENCES CHUDE(MaCD),
	FOREIGN KEY (MaNXB) REFERENCES NHAXUATBAN(MaNXB)
);

CREATE TABLE TACGIA
(
	MaTG int identity(1,1) PRIMARY KEY,
	TenTG nvarchar(50) NOT NULL,
	DiachiTG nvarchar(100),
	DienthoaiTG varchar(15),
);

CREATE TABLE VIETSACH
(
	MaTG int,
	Masach int,
	Vaitro nvarchar(30),
	CONSTRAINT PK_VIETSACH PRIMARY KEY(MaTG, Masach),
	FOREIGN KEY (MaTG) REFERENCES TACGIA(MaTG),
	FOREIGN KEY (Masach) REFERENCES SACH(Masach)
);

CREATE TABLE KHACHHANG
(
	MaKH int identity (1,1) PRIMARY KEY,
	HoTenKH nvarchar(50) NOT NULL,
	DiachiKH nvarchar(50),
	DienthoaiKH varchar(10),
	TenDN varchar(15) UNIQUE,
	Matkhau varchar(15) NOT NULL,
	Ngaysinh smalldatetime,
	Gioitinh bit DEFAULT 1,
	Email varchar(50) UNIQUE,
	Daduyet bit default 0,
);

CREATE TABLE DONDATHANG
(
	SoDH int identity(1,1) PRIMARY KEY,
	MaKH int,
	NgayDH smalldatetime,
	Trigia MONEY CHECK (Trigia > 0),
	Dagiao bit Default 0,
	Ngaygiaohang smalldatetime,
	Tennguoinhan nvarchar(50),
	Diachinhan nvarchar(50),
	Dienthoainhan varchar(15),
	HTThanhtoan bit DEFAULT 0,
	HTGiaohang bit DEFAULT 0,
	FOREIGN KEY(MaKH) REFERENCES KHACHHANG(MaKH)
);

CREATE TABLE CTDATHANG
(
	SoDH int,
	Masach int,
	Soluong int check (Soluong > 0),
	Dongia Decimal(9,2) CHECK (Dongia >= 0),
	Thanhtien AS Soluong*Dongia,
	CONSTRAINT PK_CTDATHANG PRIMARY KEY (SoDH, Masach),
	FOREIGN KEY (Masach) REFERENCES SACH(Masach),
	FOREIGN KEY (SoDH) REFERENCES DONDATHANG(SoDH)
);

