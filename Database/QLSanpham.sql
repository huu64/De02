USE QLSanpham;
GO

CREATE TABLE LoaiSP (
    MaLoai CHAR(2) PRIMARY KEY,
    TenLoai NVARCHAR(30)
);
GO

CREATE TABLE Sanpham (
    MaSP CHAR(6) PRIMARY KEY,
    TenSP NVARCHAR(30),
    NgayNhap DATETIME,
    MaLoai CHAR(2),
    FOREIGN KEY (MaLoai) REFERENCES LoaiSP(MaLoai)
);
GO

INSERT INTO LoaiSP (MaLoai, TenLoai) VALUES ('01', 'Bánh kẹo');
INSERT INTO LoaiSP (MaLoai, TenLoai) VALUES ('02', 'Giải khát');
GO


INSERT INTO Sanpham (MaSP, TenSP, NgayNhap, MaLoai) VALUES ('SP0001', 'Kẹo dẻo', '2023-01-20', '01');
INSERT INTO Sanpham (MaSP, TenSP, NgayNhap, MaLoai) VALUES ('SP0002', 'Bánh quy', '2023-02-15', '01');
INSERT INTO Sanpham (MaSP, TenSP, NgayNhap, MaLoai) VALUES ('SP0003', 'Nước ngọt', '2023-04-24', '02');
GO

SELECT * FROM LoaiSP;
GO

SELECT * FROM Sanpham;
GO