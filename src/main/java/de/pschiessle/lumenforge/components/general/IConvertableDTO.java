package de.pschiessle.lumenforge.components.general;

import java.util.Optional;

public interface IConvertableDTO<T> {
    T fromDTO();
}
