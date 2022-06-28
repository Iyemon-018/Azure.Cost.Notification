namespace Azure.Cost.Notification.Tests.Domain.Entities;

using System;
using System.Collections.Generic;
using ChainingAssertion;
using Notification.Domain.Entities;
using Xunit;

public class EntityTest
{
    public sealed class IntEntity : Entity<int>
    {
        public IntEntity()
        {
            
        }
        
        public IntEntity(int id) : base(id)
        {
            
        }
    }

    public sealed class EnumEntity : Entity<DayOfWeek>
    {
        public EnumEntity()
        {
        }

        public EnumEntity(DayOfWeek id) : base(id)
        {
            
        }
    }

    [Fact]
    public void Test_Equals_比較するオブジェクトの型が不一致の場合はfalseを返すこと()
    {
        var a = new IntEntity(1);
        var b = new EnumEntity(DayOfWeek.Monday);

        a.Equals(b).IsFalse();
    }

    public static IEnumerable<object[]> Get_Test_Equals_operator_比較するオブジェクトが一致しない場合はfalseを返すこと_Data()
    {
        yield return new object[] { null, new IntEntity() };
        yield return new object[] { new IntEntity(), null };
    }

    [Theory]
    [MemberData(nameof(Get_Test_Equals_operator_比較するオブジェクトが一致しない場合はfalseを返すこと_Data))]
    public void Test_Equals_operator_比較するオブジェクトが一致しない場合はfalseを返すこと(IntEntity a, IntEntity b)
    {
        (a == b).IsFalse();
    }

    public static IEnumerable<object[]> Get_Test_Equals_operator_比較するオブジェクトが一致する場合はtrueを返すこと_Data()
    {
        yield return new object[] { null, null };
    }

    [Theory]
    [MemberData(nameof(Get_Test_Equals_operator_比較するオブジェクトが一致する場合はtrueを返すこと_Data))]
    public void Test_Equals_operator_比較するオブジェクトが一致する場合はtrueを返すこと(IntEntity a, IntEntity b)
    {
        (a == b).IsTrue();
    }

    [Theory]
    [InlineData(default(int))]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void Test_IntEntity_Equals_Idが一致するときtrueを返すこと(int id)
    {
        var entity = new IntEntity{Id = id};

        entity.Equals(new IntEntity{Id = id}).IsTrue();
    }

    [Fact]
    public void Test_IntEntity_Equals_同一インスタンスを比較するときtrueを返すこと()
    {
        var entity = new IntEntity { Id = DateTime.Now.Millisecond };

        entity.Equals(entity).IsTrue();
    }

    [Theory]
    [InlineData(default(int))]
    public void Test_IntEntity_Equals_Idが不一致ときfalseを返すこと(int id)
    {
        var entity = new IntEntity { Id = id };

        entity.Equals(new IntEntity { Id = id + 1 }).IsFalse();
    }

    [Theory]
    [InlineData(default(int))]
    [InlineData(-91)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void Test_IntEntity_Equals_operator_Idが一致するときtrueを返すこと(int id)
    {
        var entity = new IntEntity { Id = id };

        (entity==new IntEntity { Id = id }).IsTrue();
    }

    [Fact]
    public void Test_IntEntity_Equals_operator_同一インスタンスを比較するときtrueを返すこと()
    {
        var entity = new IntEntity { Id = DateTime.Now.Millisecond };

        (entity == entity).IsTrue();
    }

    [Theory]
    [InlineData(default(int))]
    public void Test_IntEntity_Equals_operator_Idが不一致ときfalseを返すこと(int id)
    {
        var entity = new IntEntity { Id = id };

        (entity == new IntEntity { Id = id + 1 }).IsFalse();
    }

    [Theory]
    [InlineData(default(int))]
    [InlineData(9702637)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void Test_IntEntity_Not_Equals_operator_Idが一致するときfalseを返すこと(int id)
    {
        var entity = new IntEntity { Id = id };

        (entity != new IntEntity { Id = id }).IsFalse();
    }

    [Fact]
    public void Test_IntEntity_Not_Equals_operator_同一インスタンスを比較するときfalseを返すこと()
    {
        var entity = new IntEntity { Id = DateTime.Now.Millisecond };

        (entity != entity).IsFalse();
    }

    [Theory]
    [InlineData(default(int))]
    public void Test_IntEntity_Not_Equals_operator_Idが不一致ときfalseを返すこと(int id)
    {
        var entity = new IntEntity { Id = id };

        (entity != new IntEntity { Id = id + 1 }).IsTrue();
    }

    [Fact]
    public void Test_IntEntity_GetHashCode_Idの値が一致するとき同じ値を返すこと()
    {
        var entity = new IntEntity { Id = DateTime.Now.Millisecond };

        entity.GetHashCode().Is(entity.GetHashCode());
    }

    [Fact]
    public void Test_IntEntity_GetHashCode_Idの値が不一致のとき異なる値を返すこと()
    {
        var entity = new IntEntity { Id = DateTime.Now.Millisecond };
        var entity2 = new IntEntity { Id = entity.Id + 1 };

        entity.GetHashCode().IsNot(entity2.GetHashCode());
    }

    [Theory]
    [InlineData(default(DayOfWeek))]
    [InlineData(DayOfWeek.Monday)]
    [InlineData(DayOfWeek.Saturday)]
    public void Test_EnumEntity_Equals_Idが一致するときtrueを返すこと(DayOfWeek id)
    {
        var entity = new EnumEntity { Id = id };

        entity.Equals(new EnumEntity { Id = id }).IsTrue();
    }

    [Fact]
    public void Test_EnumEntity_Equals_同一インスタンスを比較するときtrueを返すこと()
    {
        var entity = new EnumEntity { Id = DateTime.Now.DayOfWeek };

        entity.Equals(entity).IsTrue();
    }

    [Theory]
    [InlineData(default(DayOfWeek))]
    public void Test_EnumEntity_Equals_Idが不一致ときfalseを返すこと(DayOfWeek id)
    {
        var entity = new EnumEntity { Id = id };

        entity.Equals(new EnumEntity { Id = id + 1 }).IsFalse();
    }

    [Theory]
    [InlineData(default(DayOfWeek))]
    [InlineData(DayOfWeek.Wednesday)]
    [InlineData(DayOfWeek.Friday)]
    public void Test_EnumEntity_Equals_operator_Idが一致するときtrueを返すこと(DayOfWeek id)
    {
        var entity = new EnumEntity { Id = id };

        (entity == new EnumEntity { Id = id }).IsTrue();
    }

    [Fact]
    public void Test_EnumEntity_Equals_operator_同一インスタンスを比較するときtrueを返すこと()
    {
        var entity = new EnumEntity { Id = DateTime.Now.DayOfWeek };

        (entity == entity).IsTrue();
    }

    [Theory]
    [InlineData(default(DayOfWeek))]
    public void Test_EnumEntity_Equals_operator_Idが不一致ときfalseを返すこと(DayOfWeek id)
    {
        var entity = new EnumEntity { Id = id };

        (entity == new EnumEntity { Id = id + 1 }).IsFalse();
    }

    [Theory]
    [InlineData(DayOfWeek.Sunday)]
    [InlineData(DayOfWeek.Thursday)]
    public void Test_EnumEntity_Not_Equals_operator_Idが一致するときfalseを返すこと(DayOfWeek id)
    {
        var entity = new EnumEntity { Id = id };

        (entity != new EnumEntity { Id = id }).IsFalse();
    }

    [Fact]
    public void Test_EnumEntity_Not_Equals_operator_同一インスタンスを比較するときfalseを返すこと()
    {
        var entity = new EnumEntity { Id = DateTime.Now.DayOfWeek };

        (entity != entity).IsFalse();
    }

    [Theory]
    [InlineData(default(DayOfWeek))]
    public void Test_EnumEntity_Not_Equals_operator_Idが不一致ときfalseを返すこと(DayOfWeek id)
    {
        var entity = new EnumEntity { Id = id };

        (entity != new EnumEntity { Id = id + 1 }).IsTrue();
    }

    [Fact]
    public void Test_EnumEntity_GetHashCode_Idの値が一致するとき同じ値を返すこと()
    {
        var entity = new EnumEntity { Id = DateTime.Now.DayOfWeek };

        entity.GetHashCode().Is(entity.GetHashCode());
    }

    [Fact]
    public void Test_EnumEntity_GetHashCode_Idの値が不一致のとき異なる値を返すこと()
    {
        var entity  = new EnumEntity { Id = DayOfWeek.Sunday };
        var entity2 = new EnumEntity { Id = DayOfWeek.Wednesday };

        entity.GetHashCode().IsNot(entity2.GetHashCode());
    }
}