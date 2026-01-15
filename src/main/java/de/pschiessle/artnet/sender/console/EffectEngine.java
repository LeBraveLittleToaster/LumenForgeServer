package de.pschiessle.artnet.sender.console;

import de.pschiessle.artnet.sender.console.persistent.Channel;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;

import java.util.List;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentMap;
import java.util.concurrent.atomic.AtomicLong;

@Component
public class EffectEngine {

    private List<Channel> channelList = List.of();

    final ConcurrentMap<Integer, Integer[]> universeToDMXMap = new ConcurrentHashMap<>();

    private AtomicLong lastTimeStamp = new AtomicLong(System.currentTimeMillis());
    private AtomicLong curTimeStamp = new AtomicLong(System.currentTimeMillis());

    public void setChannelList(List<Channel> channelList) {
        this.channelList = channelList;

        rebuildUniverseMapping(channelList);

    }

    private void rebuildUniverseMapping(List<Channel> channelList) {
        universeToDMXMap.clear();
        this.channelList.stream()
                .map(channel -> channel.getSpatialDeviceConfig().getDmxDevice().getUniverse())
                .distinct()
                .forEach((universe) -> universeToDMXMap.put(universe, new Integer[512]));
    }

    @Scheduled(fixedRate = 25)
    public void loopAt40Hz(){
        this.curTimeStamp = new AtomicLong(System.currentTimeMillis());
        for(Channel channel : this.channelList) {
            runChannelEffectStack(channel, universeToDMXMap , lastTimeStamp.get(), curTimeStamp.get());
        }
        this.lastTimeStamp.set(curTimeStamp.get());
    }

    private void runChannelEffectStack(Channel channel, ConcurrentMap<Integer, Integer[]> universeToDMXMap, long lastTimeStamp, long curTimeStamp) {
        if(channel.getEffectGeneratorStack().isEmpty()) {
            return;
        }

        return;
    }



}
